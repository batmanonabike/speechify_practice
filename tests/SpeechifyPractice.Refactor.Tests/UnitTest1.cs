using SpeechifyPractice.Refactor;

namespace SpeechifyPractice.Refactor.Tests;

public class RefactoringKataTests
{
    [Fact]
    public void FeeCalculator_CardRule_MatchesLegacyBehavior()
    {
        var sut = new PaymentFeeCalculator();

        var fee = sut.Calculate(100m, PaymentMethod.Card);

        Assert.Equal(3.20m, fee);
    }

    [Fact]
    public void FeeCalculator_BankTransferRule_IsCappedAtFiveDollars()
    {
        var sut = new PaymentFeeCalculator();

        var fee = sut.Calculate(1000m, PaymentMethod.BankTransfer);

        Assert.Equal(5.00m, fee);
    }

    [Fact]
    public void CachedRateClient_UsesCacheWithinTtl()
    {
        var clock = new FakeClock(new DateTime(2026, 08, 14, 9, 0, 0, DateTimeKind.Utc));
        var inner = new FakeRateClient(1.11m);
        var sut = new CachedCurrencyRateClient(inner, clock, TimeSpan.FromMinutes(5));

        var r1 = sut.GetUsdRate("eur");
        var r2 = sut.GetUsdRate("EUR");

        Assert.Equal(1.11m, r1);
        Assert.Equal(1.11m, r2);
        Assert.Equal(1, inner.CallCount);
    }

    [Fact]
    public void CachedRateClient_RefreshesAfterTtlExpires()
    {
        var clock = new FakeClock(new DateTime(2026, 08, 14, 9, 0, 0, DateTimeKind.Utc));
        var inner = new FakeRateClient(1.27m);
        var sut = new CachedCurrencyRateClient(inner, clock, TimeSpan.FromMinutes(5));

        _ = sut.GetUsdRate("GBP");
        clock.Advance(TimeSpan.FromMinutes(6));
        _ = sut.GetUsdRate("GBP");

        Assert.Equal(2, inner.CallCount);
    }

    [Fact]
    public void Checkout_ComputesFeeTotalAndRiskBand()
    {
        var fees = new PaymentFeeCalculator();
        var rates = new FakeRateClient(1.11m);
        var sut = new PaymentCheckoutService(fees, rates);

        var request = new PaymentRequest(
            Amount: 600m,
            Currency: "EUR",
            Method: PaymentMethod.Card,
            Country: "DE",
            CustomerId: "new_42");

        var summary = sut.Checkout(request);

        Assert.Equal(17.70m, summary.Fee);
        Assert.Equal(685.65m, summary.TotalUsd);
        Assert.Equal("HIGH", summary.RiskBand);
    }

    private sealed class FakeClock : IClock
    {
        public FakeClock(DateTime utcNow)
        {
            UtcNow = utcNow;
        }

        public DateTime UtcNow { get; private set; }

        public void Advance(TimeSpan by)
        {
            UtcNow = UtcNow.Add(by);
        }
    }

    private sealed class FakeRateClient : ICurrencyRateClient
    {
        private readonly decimal _rate;

        public FakeRateClient(decimal rate)
        {
            _rate = rate;
        }

        public int CallCount { get; private set; }

        public decimal GetUsdRate(string currencyCode)
        {
            CallCount++;
            return _rate;
        }
    }
}
