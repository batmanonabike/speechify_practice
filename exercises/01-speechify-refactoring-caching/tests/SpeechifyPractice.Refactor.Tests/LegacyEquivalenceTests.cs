using SpeechifyPractice.Legacy;
using SpeechifyPractice.Refactor;

namespace SpeechifyPractice.Refactor.Tests;

/// <summary>
/// Runs the same inputs through <see cref="LegacyPaymentProcessor"/> and through the
/// refactored service, and demands they agree.
/// </summary>
/// <remarks>
/// "Keep behaviour stable" is only a real constraint if something checks it. Nothing
/// consumed the legacy processor before, so nothing did.
/// </remarks>
public class LegacyEquivalenceTests
{
    private static readonly DateTime Start = new(2026, 08, 14, 9, 0, 0, DateTimeKind.Utc);

    public static IEnumerable<object[]> Cases() =>
    [
        [100m, "USD", "card", "US", "cust_1"],
        [100m, "EUR", "card", "DE", "cust_1"],
        [600m, "EUR", "card", "DE", "new_42"],
        [600m, "GBP", "card", "GB", "new_1"],
        [250m, "USD", "bank_transfer", "US", "cust_1"],
        [1000m, "CAD", "bank_transfer", "CA", "cust_9"],
        [200m, "USD", "wallet", "US", "cust_1"],
        [750m, "GBP", "wallet", "GB", "new_7"],
        [0m, "USD", "card", "US", "cust_1"],
        [501m, "USD", "card", "US", "cust_1"],
        [500m, "USD", "card", "US", "cust_1"],
        [12345.67m, "EUR", "card", "FR", "cust_3"],
    ];

    [Theory]
    [MemberData(nameof(Cases))]
    public void RefactoredCheckout_AgreesWithLegacyProcessor(
        decimal amount, string currency, string method, string country, string customerId)
    {
        LegacyPaymentReceipt before = new LegacyPaymentProcessor()
            .Charge(new LegacyPaymentRequest(amount, currency, method, country, customerId));

        PaymentSummary after = new PaymentCheckoutService(new PaymentFeeCalculator(), new TableRateClient())
            .Checkout(new PaymentRequest(amount, currency, ToMethod(method), country, customerId));

        Assert.Equal(before.Fee, after.Fee);
        Assert.Equal(before.TotalChargedUsd, after.TotalUsd);
        Assert.Equal(before.RiskBand, after.RiskBand);
    }

    /// <summary>
    /// The legacy processor looks the rate up on every single charge. That is the
    /// performance problem the caching decorator exists to solve.
    /// </summary>
    [Fact]
    public void LegacyProcessor_LooksUpTheRateOnEveryCharge()
    {
        var legacy = new LegacyPaymentProcessor();

        for (int i = 1; i <= 20; i++)
        {
            legacy.Charge(new LegacyPaymentRequest(i * 10m, "EUR", "card", "DE", "cust_1"));
        }

        Assert.Equal(20, legacy.ExchangeRateLookupCount);
    }

    [Fact]
    public void CachedClient_CollapsesRepeatedLookupsToOnePerCurrency()
    {
        var inner = new TableRateClient();
        var cached = new CachedCurrencyRateClient(inner, new FakeClock(Start), TimeSpan.FromMinutes(5));
        var service = new PaymentCheckoutService(new PaymentFeeCalculator(), cached);

        string[] currencies = ["USD", "EUR", "GBP", "CAD"];

        for (int i = 1; i <= 40; i++)
        {
            service.Checkout(new PaymentRequest(
                i * 10m, currencies[i % currencies.Length], PaymentMethod.Card, "US", "cust_1"));
        }

        // Legacy would have made 40 calls here.
        Assert.Equal(currencies.Length, inner.CallCount);
    }

    private static PaymentMethod ToMethod(string legacyMethod) => legacyMethod switch
    {
        "card" => PaymentMethod.Card,
        "bank_transfer" => PaymentMethod.BankTransfer,
        "wallet" => PaymentMethod.Wallet,
        _ => throw new ArgumentOutOfRangeException(nameof(legacyMethod), legacyMethod, null)
    };
}
