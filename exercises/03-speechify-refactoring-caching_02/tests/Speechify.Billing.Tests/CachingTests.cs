namespace Speechify.Billing.Tests;

/// <summary>
/// The performance half of the brief.
/// </summary>
/// <remarks>
/// The legacy cache keys on currency AND amount, so it effectively never hits —
/// see <c>CharacterizationTests.RateCache_MissesWheneverTheAmountChanges</c>,
/// where 25 charges cost 25 network calls. Your replacement must cache by
/// currency, honour a TTL, and not leak between instances.
/// </remarks>
public class CachingTests : LegacyStateIsolated
{
    private static readonly TimeSpan Ttl = TimeSpan.FromMinutes(5);

    private static IBillingService Build(FakeRateApi api, FixedTimeProvider clock) =>
        BillingComposition.Create(api, clock, Ttl);

    private static ChargeRequest Request(decimal amount, string currency) =>
        new(amount, currency, "card", "US", "cust_1", false, 400, null);

    [Fact]
    public void RepeatedCharges_InOneCurrency_HitTheRateApiOnce()
    {
        var api = new FakeRateApi();
        var service = Build(api, new FixedTimeProvider(Weekday));

        for (int i = 1; i <= 25; i++)
        {
            service.Charge(Request(i, "EUR"));
        }

        // The legacy engine makes 25 calls here because its cache key includes
        // the amount. Yours should make one.
        Assert.Equal(1, api.CallCount);
    }

    [Fact]
    public void Batch_AcrossFourCurrencies_HitsTheRateApiAtMostFourTimes()
    {
        var api = new FakeRateApi();
        var service = Build(api, new FixedTimeProvider(Weekday));

        string[] currencies = ["USD", "EUR", "GBP", "CAD"];
        var requests = Enumerable.Range(1, 40)
            .Select(i => Request(i * 3m, currencies[i % currencies.Length]))
            .ToList();

        service.ChargeBatch(requests);

        Assert.True(
            api.CallCount <= 4,
            $"40 charges over 4 currencies should cost at most 4 rate lookups, but cost {api.CallCount}.");
    }

    [Fact]
    public void CachedRate_IsReusedWithinTtl()
    {
        var api = new FakeRateApi();
        var clock = new FixedTimeProvider(Weekday);
        var service = Build(api, clock);

        service.Charge(Request(100m, "EUR"));
        clock.Advance(TimeSpan.FromMinutes(4));
        service.Charge(Request(200m, "EUR"));

        Assert.Equal(1, api.CallCount);
    }

    [Fact]
    public void CachedRate_IsRefreshedAfterTtlExpires()
    {
        var api = new FakeRateApi();
        var clock = new FixedTimeProvider(Weekday);
        var service = Build(api, clock);

        service.Charge(Request(100m, "EUR"));
        clock.Advance(TimeSpan.FromMinutes(6));
        service.Charge(Request(200m, "EUR"));

        Assert.Equal(2, api.CallCount);
    }

    /// <summary>
    /// Pins the boundary explicitly: an entry is valid while age &lt; ttl, so at
    /// exactly the TTL it is already stale.
    /// </summary>
    [Fact]
    public void CachedRate_ExpiresExactlyAtTtl()
    {
        var api = new FakeRateApi();
        var clock = new FixedTimeProvider(Weekday);
        var service = Build(api, clock);

        service.Charge(Request(100m, "EUR"));

        clock.Advance(Ttl - TimeSpan.FromTicks(1));
        service.Charge(Request(100m, "EUR"));
        Assert.Equal(1, api.CallCount);

        clock.Advance(TimeSpan.FromTicks(1));
        service.Charge(Request(100m, "EUR"));
        Assert.Equal(2, api.CallCount);
    }

    [Fact]
    public void CachedRate_IsPerCurrency()
    {
        var api = new FakeRateApi();
        var service = Build(api, new FixedTimeProvider(Weekday));

        ChargeReceipt eur = service.Charge(Request(100m, "EUR"));
        ChargeReceipt gbp = service.Charge(Request(100m, "GBP"));

        Assert.Equal(2, api.CallCount);
        Assert.NotEqual(eur.TotalUsd, gbp.TotalUsd);
    }

    [Theory]
    [InlineData("EUR")]
    [InlineData("eur")]
    [InlineData(" eur ")]
    [InlineData("Eur")]
    public void CurrencyCode_IsNormalisedBeforeCaching(string variant)
    {
        var api = new FakeRateApi();
        var service = Build(api, new FixedTimeProvider(Weekday));

        service.Charge(Request(100m, "EUR"));
        service.Charge(Request(200m, variant));

        Assert.Equal(1, api.CallCount);
    }

    [Fact]
    public void CachedRate_IsUsedEvenAfterTheSourceChanges()
    {
        var api = new FakeRateApi();
        var clock = new FixedTimeProvider(Weekday);
        var service = Build(api, clock);

        ChargeReceipt first = service.Charge(Request(100m, "EUR"));

        api.SetRate("EUR", 99m);
        ChargeReceipt second = service.Charge(Request(100m, "EUR"));

        Assert.Equal(first.TotalUsd, second.TotalUsd);
        Assert.Equal(1, api.CallCount);

        clock.Advance(Ttl);
        ChargeReceipt third = service.Charge(Request(100m, "EUR"));
        Assert.NotEqual(first.TotalUsd, third.TotalUsd);
    }

    /// <summary>
    /// Two services must not share state. The legacy engine fails this because
    /// its dictionary is static.
    /// </summary>
    [Fact]
    public void SeparateServices_DoNotShareACache()
    {
        var api = new FakeRateApi();
        var clock = new FixedTimeProvider(Weekday);

        Build(api, clock).Charge(Request(100m, "EUR"));
        Assert.Equal(1, api.CallCount);

        Build(api, clock).Charge(Request(100m, "EUR"));
        Assert.Equal(2, api.CallCount);
    }

    /// <summary>
    /// STRETCH GOAL. Concurrent misses on a cold cache should collapse into a
    /// single upstream call rather than a stampede.
    /// </summary>
    [Fact]
    public async Task ConcurrentColdCalls_CollapseIntoOneRateLookup()
    {
        var api = new FakeRateApi { LatencyMs = 40 };
        var service = Build(api, new FixedTimeProvider(Weekday));

        using var start = new ManualResetEventSlim(false);

        Task<ChargeReceipt>[] tasks = Enumerable.Range(0, 64)
            .Select(i => Task.Run(() =>
            {
                start.Wait();
                return service.Charge(Request(100m + i, "EUR"));
            }))
            .ToArray();

        start.Set();
        ChargeReceipt[] receipts = await Task.WhenAll(tasks);

        Assert.Equal(1, api.CallCount);
        Assert.All(receipts, r => Assert.Equal("EUR", r.Currency));
    }

    /// <summary>
    /// A plain Dictionary written from several threads can corrupt its buckets or
    /// spin forever. Whatever you cache in has to survive this.
    /// </summary>
    /// <remarks>
    /// This does NOT require single-flight — a ConcurrentDictionary is allowed to
    /// run its value factory more than once for the same key. It only requires that
    /// concurrency never produces a wrong or torn answer. The stricter one-call
    /// guarantee is the separate stretch test above.
    /// </remarks>
    [Fact]
    public void ConcurrentCharges_NeverProduceAWrongAnswer()
    {
        var api = new FakeRateApi();
        var service = Build(api, new FixedTimeProvider(Weekday));
        string[] currencies = ["USD", "EUR", "GBP", "CAD", "JPY"];
        var results = new System.Collections.Concurrent.ConcurrentBag<ChargeReceipt>();

        var completed = Parallel.For(0, 500, i =>
        {
            results.Add(service.Charge(Request(100m, currencies[i % currencies.Length])));
        });

        Assert.True(completed.IsCompleted);
        Assert.Equal(500, results.Count);

        // Identical inputs must give identical output regardless of interleaving.
        foreach (string currency in currencies)
        {
            decimal[] totals = results
                .Where(r => r.Currency == currency)
                .Select(r => r.TotalUsd)
                .Distinct()
                .ToArray();

            Assert.True(
                totals.Length == 1,
                $"{currency} produced inconsistent totals: {string.Join(", ", totals)}");
        }
    }
}
