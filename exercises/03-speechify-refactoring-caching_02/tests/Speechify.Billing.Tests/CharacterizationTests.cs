using Speechify.Billing.Abstractions;
using Speechify.Billing.Legacy;

namespace Speechify.Billing.Tests;

/// <summary>
/// Pins the observable behaviour of <see cref="BillingEngine"/> exactly as it is today.
/// </summary>
/// <remarks>
/// <para>
/// These are GREEN before you write a line of code, and they must stay green.
/// They are not a specification of what billing *should* do — they are a record
/// of what it *does*, warts included. Several rows pin behaviour that looks like
/// a bug. Preserving it is the job; changing it is a separate conversation.
/// </para>
/// <para>
/// Every expected value here was produced by executing the legacy engine, not by
/// hand-calculation.
/// </para>
/// </remarks>
public class CharacterizationTests : LegacyStateIsolated
{
    // 14 columns, so this is a plain object[] sequence: xUnit's TheoryData only
    // has generic overloads up to 10 type arguments.
    public static IEnumerable<object[]> ChargeCases() => Rows;

    //  amount, currency, method, country, customerId, isSubscription, ageDays, promo, isWeekend,
    //  => expected fee, surcharge, totalUsd, riskBand, currency
    private static readonly object[][] Rows =
    [
        // --- base fee rules -------------------------------------------------
        new object[] { 100m, "USD", "card", "US", "cust_1", false, 400, null, false, 3.20m, 0m, 103.20m, "LOW", "USD" },
        // The next three land exactly on a half-cent. ProcessCharge uses banker's
        // rounding here, so they round DOWN to even. Do not "tidy" this away.
        new object[] { 5m, "USD", "card", "US", "cust_1", false, 400, null, false, 0.44m, 0m, 5.44m, "LOW", "USD" },
        new object[] { 25m, "USD", "card", "US", "cust_1", false, 400, null, false, 1.02m, 0m, 26.02m, "LOW", "USD" },
        new object[] { 45m, "USD", "card", "US", "cust_1", false, 400, null, false, 1.60m, 0m, 46.60m, "LOW", "USD" },
        new object[] { 3m, "USD", "wallet", "US", "cust_1", false, 400, null, false, 0.04m, 0m, 3.04m, "LOW", "USD" },
        new object[] { 7m, "USD", "wallet", "US", "cust_1", false, 400, null, false, 0.10m, 0m, 7.10m, "LOW", "USD" },
        new object[] { 0.5m, "USD", "bank_transfer", "US", "cust_1", false, 400, null, false, 0.00m, 0m, 0.50m, "LOW", "USD" },
        new object[] { 2.5m, "USD", "bank_transfer", "US", "cust_1", false, 400, null, false, 0.02m, 0m, 2.52m, "LOW", "USD" },
        new object[] { 1000m, "USD", "bank_transfer", "US", "cust_1", false, 400, null, false, 5m, 0m, 1005m, "MEDIUM", "USD" },
        new object[] { 250m, "USD", "bank_transfer", "US", "cust_1", false, 400, null, false, 2.50m, 0m, 252.50m, "LOW", "USD" },

        // --- currency conversion --------------------------------------------
        new object[] { 600m, "EUR", "card", "DE", "new_42", false, 10, null, false, 17.70m, 0m, 685.65m, "CRITICAL", "EUR" },
        new object[] { 600m, "EUR", "card", "NG", "new_42", false, 10, null, false, 17.70m, 0m, 685.65m, "CRITICAL", "EUR" },
        new object[] { 120m, "GBP", "wallet", "GB", "cust_9", false, 200, null, false, 1.80m, 0m, 154.69m, "MEDIUM", "GBP" },
        new object[] { 80m, "CAD", "card", "CA", "cust_9", false, 200, null, false, 2.62m, 0m, 61.14m, "LOW", "CAD" },
        // Fee is charged in the source currency, then the whole lot is converted.
        new object[] { 15000m, "JPY", "card", "JP", "cust_9", false, 200, null, false, 435.30m, 0m, 103.42m, "HIGH", "JPY" },

        // --- subscription discount tiers -------------------------------------
        new object[] { 100m, "USD", "card", "US", "cust_1", true, 400, null, false, 1.60m, 0m, 101.60m, "LOW", "USD" },
        new object[] { 100m, "USD", "card", "US", "cust_1", true, 100, null, false, 2.40m, 0m, 102.40m, "LOW", "USD" },
        new object[] { 100m, "USD", "card", "US", "cust_1", true, 10, null, false, 2.88m, 0m, 102.88m, "LOW", "USD" },
        new object[] { 100m, "USD", "card", "US", "cust_1", true, 366, null, false, 1.60m, 0m, 101.60m, "LOW", "USD" },
        new object[] { 100m, "USD", "card", "US", "cust_1", true, 365, null, false, 2.40m, 0m, 102.40m, "LOW", "USD" },
        new object[] { 100m, "USD", "card", "US", "cust_1", true, 91, null, false, 2.40m, 0m, 102.40m, "LOW", "USD" },
        new object[] { 100m, "USD", "card", "US", "cust_1", true, 90, null, false, 2.88m, 0m, 102.88m, "LOW", "USD" },

        // --- promo code -------------------------------------------------------
        new object[] { 100m, "USD", "card", "US", "cust_1", false, 400, "WAIVEFEE", false, 0m, 0m, 100m, "LOW", "USD" },
        new object[] { 100m, "USD", "card", "US", "cust_1", false, 400, "waivefee", false, 0m, 0m, 100m, "LOW", "USD" },
        new object[] { 100m, "USD", "card", "US", "cust_1", false, 400, "OTHER", false, 3.20m, 0m, 103.20m, "LOW", "USD" },

        // --- weekend surcharge -------------------------------------------------
        new object[] { 100m, "USD", "card", "US", "cust_1", false, 400, null, true, 3.20m, 0.50m, 103.70m, "LOW", "USD" },
        new object[] { 600m, "EUR", "card", "DE", "new_42", false, 10, null, true, 17.70m, 3.00m, 688.98m, "CRITICAL", "EUR" },
        new object[] { 3m, "USD", "wallet", "US", "cust_1", false, 400, null, true, 0.04m, 0.02m, 3.06m, "LOW", "USD" },
        new object[] { 1m, "USD", "wallet", "US", "cust_1", false, 400, null, true, 0.02m, 0.00m, 1.02m, "LOW", "USD" },

        // --- risk band boundaries ----------------------------------------------
        new object[] { 500m, "USD", "card", "US", "cust_1", false, 400, null, false, 14.80m, 0m, 514.80m, "LOW", "USD" },
        new object[] { 501m, "USD", "card", "US", "cust_1", false, 400, null, false, 14.83m, 0m, 515.83m, "MEDIUM", "USD" },
        new object[] { 101m, "USD", "card", "US", "cust_1", false, 400, null, false, 3.23m, 0m, 104.23m, "LOW", "USD" },
        new object[] { 100m, "USD", "card", "US", "cust_1", false, 30, null, false, 3.20m, 0m, 103.20m, "LOW", "USD" },
        new object[] { 100m, "USD", "card", "US", "cust_1", false, 29, null, false, 3.20m, 0m, 103.20m, "LOW", "USD" },
        new object[] { 100m, "USD", "card", "us", "cust_1", false, 400, null, false, 3.20m, 0m, 103.20m, "LOW", "USD" },
        new object[] { 100m, "USD", "card", "DE", "cust_1", false, 400, null, false, 3.20m, 0m, 103.20m, "LOW", "USD" },
        new object[] { 100m, "USD", "card", "NG", "cust_1", false, 400, null, false, 3.20m, 0m, 103.20m, "HIGH", "USD" },
        new object[] { 100m, "USD", "card", "US", "NEW_1", false, 400, null, false, 3.20m, 0m, 103.20m, "LOW", "USD" },
        new object[] { 600m, "USD", "card", "NG", "new_1", false, 10, null, false, 17.70m, 0m, 617.70m, "CRITICAL", "USD" },

        // --- amount edges --------------------------------------------------------
        new object[] { 0m, "USD", "card", "US", "cust_1", false, 400, null, false, 0.30m, 0m, 0.30m, "LOW", "USD" },
        new object[] { 0.01m, "USD", "wallet", "US", "cust_1", false, 400, null, false, 0.00m, 0m, 0.01m, "LOW", "USD" },
        new object[] { 99999m, "USD", "card", "US", "cust_1", false, 400, null, false, 2900.27m, 0m, 102899.27m, "MEDIUM", "USD" },

        // --- input casing and whitespace -------------------------------------------
        new object[] { 100m, "eur", "CARD", "US", "cust_1", false, 400, null, false, 3.20m, 0m, 114.55m, "LOW", "EUR" },
        new object[] { 100m, " eur ", " card ", "US", "cust_1", false, 400, null, false, 3.20m, 0m, 114.55m, "LOW", "EUR" },
    ];

    [Theory]
    [MemberData(nameof(ChargeCases))]
    public void ProcessCharge_MatchesRecordedBehaviour(
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
        PinLegacyClock(isWeekend ? Weekend : Weekday);
        var engine = new BillingEngine(new FakeRateApi());

        LegacyReceipt receipt = engine.ProcessCharge(
            amount, currency, method, country, customerId, isSubscription, accountAgeDays, promoCode!);

        Assert.Equal(expectedFee, receipt.Fee);
        Assert.Equal(expectedSurcharge, receipt.Surcharge);
        Assert.Equal(expectedTotalUsd, receipt.TotalUsd);
        Assert.Equal(expectedRiskBand, receipt.RiskBand);
        Assert.Equal(expectedCurrency, receipt.Currency);
        Assert.Equal(amount, receipt.Amount);
    }

    // amount, method, expected estimate
    public static IEnumerable<object[]> EstimateCases() =>
    [
        [100m, "card", 3.20m],
        [5m, "card", 0.45m],              // ProcessCharge actually charges 0.44
        [25m, "card", 1.03m],             // ProcessCharge actually charges 1.02
        [45m, "card", 1.61m],             // ProcessCharge actually charges 1.60
        [3m, "wallet", 0.05m],            // ProcessCharge actually charges 0.04
        [7m, "wallet", 0.11m],            // ProcessCharge actually charges 0.10
        [1m, "wallet", 0.02m],
        [0.01m, "wallet", 0.00m],
        [0.5m, "bank_transfer", 0.01m],   // ProcessCharge actually charges 0.00
        [2.5m, "bank_transfer", 0.03m],   // ProcessCharge actually charges 0.02
        [1000m, "bank_transfer", 5m],
        [250m, "bank_transfer", 2.50m],
        [0m, "card", 0.30m],
        [99999m, "card", 2900.27m],
    ];

    /// <summary>
    /// <c>EstimateFee</c> is a second, diverged copy of the fee rules. It rounds
    /// away from zero where <c>ProcessCharge</c> rounds to even, and it applies the
    /// bank transfer cap after rounding rather than before.
    /// </summary>
    /// <remarks>
    /// The customer is quoted one number and charged another. That is a real defect,
    /// but it is the SHIPPED behaviour — pin it, then decide deliberately what to do.
    /// </remarks>
    [Theory]
    [MemberData(nameof(EstimateCases))]
    public void EstimateFee_MatchesRecordedBehaviour(decimal amount, string method, decimal expected)
    {
        var engine = new BillingEngine(new FakeRateApi());

        Assert.Equal(expected, engine.EstimateFee(amount, method));
    }

    [Theory]
    [InlineData(5, "card")]
    [InlineData(25, "card")]
    [InlineData(3, "wallet")]
    [InlineData(7, "wallet")]
    public void EstimateFee_QuotesMoreThanProcessChargeActuallyTakes(int amount, string method)
    {
        PinLegacyClock(Weekday);
        var engine = new BillingEngine(new FakeRateApi());

        decimal quoted = engine.EstimateFee(amount, method);
        decimal charged = engine
            .ProcessCharge(amount, "USD", method, "US", "cust_1", false, 400, null!)
            .Fee;

        Assert.Equal(0.01m, quoted - charged);
    }

    [Fact]
    public void ProcessCharge_WhenRateApiThrows_SilentlyFallsBackToParity()
    {
        PinLegacyClock(Weekday);
        var engine = new BillingEngine(new ThrowingRateApi());

        LegacyReceipt receipt = engine.ProcessCharge(
            100m, "EUR", "card", "US", "cust_1", false, 400, null!);

        // No exception surfaces and no signal is logged: the caller is quietly
        // billed at a 1:1 rate. Pin it so a refactor cannot change it by accident.
        Assert.Equal(103.20m, receipt.TotalUsd);
    }

    [Fact]
    public void ProcessCharge_RejectsBlankCurrency()
    {
        PinLegacyClock(Weekday);
        var engine = new BillingEngine(new FakeRateApi());

        Assert.Throws<ArgumentException>(() =>
            engine.ProcessCharge(100m, "  ", "card", "US", "cust_1", false, 400, null!));
    }

    [Fact]
    public void ProcessCharge_RejectsNegativeAmount()
    {
        PinLegacyClock(Weekday);
        var engine = new BillingEngine(new FakeRateApi());

        Assert.Throws<ArgumentException>(() =>
            engine.ProcessCharge(-1m, "USD", "card", "US", "cust_1", false, 400, null!));
    }

    [Fact]
    public void ProcessCharge_RejectsUnknownMethod()
    {
        PinLegacyClock(Weekday);
        var engine = new BillingEngine(new FakeRateApi());

        Assert.Throws<ArgumentException>(() =>
            engine.ProcessCharge(100m, "USD", "crypto", "US", "cust_1", false, 400, null!));
    }

    [Fact]
    public void ProcessCharge_NullCustomerId_ThrowsNullReference()
    {
        PinLegacyClock(Weekday);
        var engine = new BillingEngine(new FakeRateApi());

        // customerId is never validated, so it blows up deep inside risk scoring
        // with a NullReferenceException rather than a useful ArgumentException.
        Assert.Throws<NullReferenceException>(() =>
            engine.ProcessCharge(100m, "USD", "card", "US", null!, false, 400, null!));
    }

    [Fact]
    public void RateCache_LeaksAcrossInstances()
    {
        PinLegacyClock(Weekday);
        var api = new FakeRateApi();

        var first = new BillingEngine(api);
        first.ProcessCharge(100m, "EUR", "card", "US", "cust_1", false, 400, null!);
        Assert.Equal(1, api.CallCount);

        // A brand new engine, yet it reuses the first one's cached rate: the
        // dictionary is static. Two tenants share one cache.
        var second = new BillingEngine(api);
        second.ProcessCharge(100m, "EUR", "card", "US", "cust_1", false, 400, null!);
        Assert.Equal(1, api.CallCount);
        Assert.Equal(0, second.RateLookupCount);
    }

    [Fact]
    public void RateCache_MissesWheneverTheAmountChanges()
    {
        PinLegacyClock(Weekday);
        var api = new FakeRateApi();
        var engine = new BillingEngine(api);

        for (int i = 1; i <= 25; i++)
        {
            engine.ProcessCharge(i, "EUR", "card", "US", "cust_1", false, 400, null!);
        }

        // The cache key is currency + amount, so it only ever helps when the exact
        // same amount is billed twice. In production traffic it never hits, the
        // dictionary grows without bound, and every charge pays for a network call.
        Assert.Equal(25, api.CallCount);
    }

    private sealed class ThrowingRateApi : IRateApi
    {
        public decimal GetUsdRate(string currencyCode) =>
            throw new InvalidOperationException("rate provider unavailable");
    }
}
