using SpeechifyPractice.Refactor;

namespace SpeechifyPractice.Refactor.Tests;

public class PaymentCheckoutServiceTests
{
    private static PaymentCheckoutService Build(ICurrencyRateClient? rates = null) =>
        new(new PaymentFeeCalculator(), rates ?? new TableRateClient());

    [Fact]
    public void ComputesFeeTotalAndRiskBand()
    {
        var sut = new PaymentCheckoutService(new PaymentFeeCalculator(), new FakeRateClient(1.11m));

        var summary = sut.Checkout(new PaymentRequest(
            Amount: 600m,
            Currency: "EUR",
            Method: PaymentMethod.Card,
            Country: "DE",
            CustomerId: "new_42"));

        Assert.Equal(17.70m, summary.Fee);
        Assert.Equal(685.65m, summary.TotalUsd);
        Assert.Equal("HIGH", summary.RiskBand);
    }

    [Fact]
    public void NullRequest_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => Build().Checkout(null!));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void BlankCurrency_Throws(string? currency)
    {
        var request = new PaymentRequest(100m, currency!, PaymentMethod.Card, "US", "cust_1");

        Assert.ThrowsAny<ArgumentException>(() => Build().Checkout(request));
    }

    [Fact]
    public void UnsupportedCurrency_Throws()
    {
        var request = new PaymentRequest(100m, "XYZ", PaymentMethod.Card, "US", "cust_1");

        Assert.ThrowsAny<ArgumentException>(() => Build().Checkout(request));
    }

    [Fact]
    public void UsdRequest_IsNotConverted()
    {
        var summary = Build().Checkout(
            new PaymentRequest(100m, "USD", PaymentMethod.Card, "US", "cust_1"));

        Assert.Equal(3.20m, summary.Fee);
        Assert.Equal(103.20m, summary.TotalUsd);
        Assert.Equal("LOW", summary.RiskBand);
    }

    [Fact]
    public void WalletAndBankTransfer_AreOrchestratedToo()
    {
        var wallet = Build().Checkout(
            new PaymentRequest(200m, "USD", PaymentMethod.Wallet, "US", "cust_1"));
        Assert.Equal(3.00m, wallet.Fee);
        Assert.Equal(203.00m, wallet.TotalUsd);

        var bank = Build().Checkout(
            new PaymentRequest(1000m, "USD", PaymentMethod.BankTransfer, "US", "cust_1"));
        Assert.Equal(5.00m, bank.Fee);
        Assert.Equal(1005.00m, bank.TotalUsd);
    }

    [Theory]
    [InlineData(100, "US", "cust_1", "LOW")]
    [InlineData(600, "US", "cust_1", "MEDIUM")]
    [InlineData(100, "DE", "cust_1", "LOW")]
    [InlineData(600, "DE", "cust_1", "HIGH")]
    [InlineData(100, "US", "new_1", "LOW")]
    [InlineData(100, "DE", "new_1", "MEDIUM")]
    [InlineData(600, "DE", "new_1", "HIGH")]
    [InlineData(500, "US", "cust_1", "LOW")]
    [InlineData(501, "US", "cust_1", "MEDIUM")]
    public void RiskBand_CoversTheScoringBoundaries(
        int amount, string country, string customerId, string expected)
    {
        var summary = Build().Checkout(
            new PaymentRequest(amount, "USD", PaymentMethod.Card, country, customerId));

        Assert.Equal(expected, summary.RiskBand);
    }
}
