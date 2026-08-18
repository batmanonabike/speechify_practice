using Speechify.Billing.Abstractions;
using Speechify.Billing.Legacy;

namespace Speechify.Billing.Tests;

/// <summary>
/// Runs the same matrix through the legacy engine and through your replacement,
/// and demands they agree field for field.
/// </summary>
/// <remarks>
/// This is the test that makes the exercise a refactor rather than a rewrite.
/// It compares against the legacy engine live, so it cannot drift from the oracle.
/// </remarks>
public class EquivalenceTests : LegacyStateIsolated
{
    private static (BillingEngine Legacy, IBillingService Replacement) Build(bool isWeekend, FakeRateApi api)
    {
        DateTimeOffset instant = isWeekend ? Weekend : Weekday;
        PinLegacyClock(instant);

        var replacement = BillingComposition.Create(
            api,
            new FixedTimeProvider(instant),
            TimeSpan.FromMinutes(5));

        return (new BillingEngine(api), replacement);
    }

    [Theory]
    [MemberData(nameof(CharacterizationTests.ChargeCases), MemberType = typeof(CharacterizationTests))]
    public void Charge_AgreesWithLegacy(
        decimal amount,
        string currency,
        string method,
        string country,
        string customerId,
        bool isSubscription,
        int accountAgeDays,
        string? promoCode,
        bool isWeekend,
        decimal expectedFee,
        decimal expectedSurcharge,
        decimal expectedTotalUsd,
        string expectedRiskBand,
        string expectedCurrency)
    {
        var (legacy, replacement) = Build(isWeekend, new FakeRateApi());

        LegacyReceipt before = legacy.ProcessCharge(
            amount, currency, method, country, customerId, isSubscription, accountAgeDays, promoCode!);

        ChargeReceipt after = replacement.Charge(new ChargeRequest(
            amount, currency, method, country, customerId, isSubscription, accountAgeDays, promoCode));

        Assert.Equal(before.Amount, after.Amount);
        Assert.Equal(before.Fee, after.Fee);
        Assert.Equal(before.Surcharge, after.Surcharge);
        Assert.Equal(before.TotalUsd, after.TotalUsd);
        Assert.Equal(before.RiskBand, after.RiskBand);
        Assert.Equal(before.Currency, after.Currency);

        // Belt and braces: also match the values recorded in CharacterizationTests.
        Assert.Equal(expectedFee, after.Fee);
        Assert.Equal(expectedSurcharge, after.Surcharge);
        Assert.Equal(expectedTotalUsd, after.TotalUsd);
        Assert.Equal(expectedRiskBand, after.RiskBand);
        Assert.Equal(expectedCurrency, after.Currency);
    }

    [Theory]
    [MemberData(nameof(CharacterizationTests.EstimateCases), MemberType = typeof(CharacterizationTests))]
    public void EstimateFee_AgreesWithLegacy(decimal amount, string method, decimal expected)
    {
        var (legacy, replacement) = Build(isWeekend: false, new FakeRateApi());

        Assert.Equal(legacy.EstimateFee(amount, method), replacement.EstimateFee(amount, method));
        Assert.Equal(expected, replacement.EstimateFee(amount, method));
    }

    [Fact]
    public void ChargeBatch_AgreesWithLegacy()
    {
        var api = new FakeRateApi();
        var (legacy, replacement) = Build(isWeekend: false, api);

        var requests = new List<ChargeRequest>();
        var legacyRows = new List<object[]>();

        for (int i = 1; i <= 20; i++)
        {
            decimal amount = 10m * i;
            string currency = (i % 4) switch { 0 => "USD", 1 => "EUR", 2 => "GBP", _ => "CAD" };
            string method = (i % 3) switch { 0 => "card", 1 => "wallet", _ => "bank_transfer" };

            requests.Add(new ChargeRequest(amount, currency, method, "US", "cust_" + i, false, 400, null));
            legacyRows.Add([amount, currency, method, "US", "cust_" + i, false, 400, null!]);
        }

        List<LegacyReceipt> before = legacy.ProcessBatch(legacyRows);
        IReadOnlyList<ChargeReceipt> after = replacement.ChargeBatch(requests);

        Assert.Equal(before.Count, after.Count);

        for (int i = 0; i < before.Count; i++)
        {
            Assert.Equal(before[i].Fee, after[i].Fee);
            Assert.Equal(before[i].Surcharge, after[i].Surcharge);
            Assert.Equal(before[i].TotalUsd, after[i].TotalUsd);
            Assert.Equal(before[i].RiskBand, after[i].RiskBand);
            Assert.Equal(before[i].Currency, after[i].Currency);
        }
    }

    [Fact]
    public void Charge_WhenRateApiThrows_MatchesLegacyFallback()
    {
        PinLegacyClock(Weekday);
        var api = new ThrowingRateApi();

        var legacy = new BillingEngine(api);
        var replacement = BillingComposition.Create(
            api, new FixedTimeProvider(Weekday), TimeSpan.FromMinutes(5));

        LegacyReceipt before = legacy.ProcessCharge(100m, "EUR", "card", "US", "cust_1", false, 400, null!);
        ChargeReceipt after = replacement.Charge(
            new ChargeRequest(100m, "EUR", "card", "US", "cust_1", false, 400, null));

        Assert.Equal(before.TotalUsd, after.TotalUsd);
    }

    private sealed class ThrowingRateApi : IRateApi
    {
        public decimal GetUsdRate(string currencyCode) =>
            throw new InvalidOperationException("rate provider unavailable");
    }
}
