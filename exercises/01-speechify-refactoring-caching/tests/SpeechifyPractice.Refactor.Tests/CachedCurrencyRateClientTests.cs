using SpeechifyPractice.Refactor;

namespace SpeechifyPractice.Refactor.Tests;

public class CachedCurrencyRateClientTests
{
    private static readonly DateTime Start = new(2026, 08, 14, 9, 0, 0, DateTimeKind.Utc);
    private static readonly TimeSpan Ttl = TimeSpan.FromMinutes(5);

    private static CachedCurrencyRateClient Build(ICurrencyRateClient inner, FakeClock clock) =>
        new(inner, clock, Ttl);

    [Fact]
    public void UsesCacheWithinTtl()
    {
        var inner = new FakeRateClient(1.11m);
        var sut = Build(inner, new FakeClock(Start));

        Assert.Equal(1.11m, sut.GetUsdRate("eur"));
        Assert.Equal(1.11m, sut.GetUsdRate("EUR"));
        Assert.Equal(1, inner.CallCount);
    }

    [Fact]
    public void RefreshesAfterTtlExpires()
    {
        var inner = new FakeRateClient(1.27m);
        var clock = new FakeClock(Start);
        var sut = Build(inner, clock);

        _ = sut.GetUsdRate("GBP");
        clock.Advance(TimeSpan.FromMinutes(6));
        _ = sut.GetUsdRate("GBP");

        Assert.Equal(2, inner.CallCount);
    }

    /// <summary>
    /// Pins the boundary the original test left undefined: an entry is valid while
    /// age &lt; ttl, so at exactly the TTL it is already stale.
    /// </summary>
    [Fact]
    public void ExpiresExactlyAtTtl()
    {
        var inner = new FakeRateClient(1.11m);
        var clock = new FakeClock(Start);
        var sut = Build(inner, clock);

        _ = sut.GetUsdRate("EUR");

        clock.Advance(Ttl - TimeSpan.FromTicks(1));
        _ = sut.GetUsdRate("EUR");
        Assert.Equal(1, inner.CallCount);

        clock.Advance(TimeSpan.FromTicks(1));
        _ = sut.GetUsdRate("EUR");
        Assert.Equal(2, inner.CallCount);
    }

    [Fact]
    public void DifferentCurrencies_DoNotShareACacheEntry()
    {
        var inner = new TableRateClient();
        var sut = Build(inner, new FakeClock(Start));

        Assert.Equal(1.11m, sut.GetUsdRate("EUR"));
        Assert.Equal(1.27m, sut.GetUsdRate("GBP"));
        Assert.Equal(0.74m, sut.GetUsdRate("CAD"));
        Assert.Equal(3, inner.CallCount);

        Assert.Equal(1.11m, sut.GetUsdRate("EUR"));
        Assert.Equal(3, inner.CallCount);
    }

    [Theory]
    [InlineData("eur")]
    [InlineData("EUR")]
    [InlineData("Eur")]
    [InlineData(" eur ")]
    [InlineData("eur ")]
    public void CurrencyCode_IsNormalisedBeforeCaching(string variant)
    {
        var inner = new FakeRateClient(1.11m);
        var sut = Build(inner, new FakeClock(Start));

        _ = sut.GetUsdRate("EUR");
        _ = sut.GetUsdRate(variant);

        Assert.Equal(1, inner.CallCount);
    }

    [Fact]
    public void UnsupportedCurrency_PropagatesFromInnerClient()
    {
        var sut = Build(new TableRateClient(), new FakeClock(Start));

        Assert.ThrowsAny<ArgumentException>(() => sut.GetUsdRate("XYZ"));
    }

    [Fact]
    public void FailedLookup_IsNotCachedAsASuccess()
    {
        var inner = new TableRateClient();
        var sut = Build(inner, new FakeClock(Start));

        Assert.ThrowsAny<ArgumentException>(() => sut.GetUsdRate("XYZ"));
        Assert.ThrowsAny<ArgumentException>(() => sut.GetUsdRate("XYZ"));

        // A failure must not leave a poisoned entry behind, and it must not be
        // swallowed on the second call either: the inner client sees both attempts.
        Assert.Equal(2, inner.CallCount);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void BlankCurrencyCode_Throws(string? currencyCode)
    {
        var sut = Build(new TableRateClient(), new FakeClock(Start));

        Assert.ThrowsAny<ArgumentException>(() => sut.GetUsdRate(currencyCode!));
    }

    [Fact]
    public void NullInnerClient_IsRejected()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new CachedCurrencyRateClient(null!, new FakeClock(Start), Ttl));
    }

    [Fact]
    public void NullClock_IsRejected()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new CachedCurrencyRateClient(new TableRateClient(), null!, Ttl));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void NonPositiveTtl_IsRejected(int minutes)
    {
        Assert.ThrowsAny<ArgumentException>(() =>
            new CachedCurrencyRateClient(
                new TableRateClient(), new FakeClock(Start), TimeSpan.FromMinutes(minutes)));
    }

    /// <summary>
    /// STRETCH GOAL. A plain Dictionary written from several threads can throw or
    /// corrupt its buckets. Whatever you cache in has to survive this.
    /// </summary>
    [Fact]
    public void ConcurrentAccess_DoesNotCorruptTheCache()
    {
        var inner = new TableRateClient();
        var sut = Build(inner, new FakeClock(Start));
        string[] codes = ["USD", "EUR", "GBP", "CAD"];

        var result = Parallel.For(0, 1000, i =>
        {
            _ = sut.GetUsdRate(codes[i % codes.Length]);
        });

        Assert.True(result.IsCompleted);
        Assert.Equal(1.11m, sut.GetUsdRate("EUR"));
    }
}
