using SpeechifyPractice.Refactor;

namespace SpeechifyPractice.Refactor.Tests;

public class PaymentFeeCalculatorTests
{
    private static readonly PaymentFeeCalculator Sut = new();

    [Fact]
    public void CardRule_MatchesLegacyBehavior()
    {
        Assert.Equal(3.20m, Sut.Calculate(100m, PaymentMethod.Card));
    }

    [Fact]
    public void BankTransferRule_IsCappedAtFiveDollars()
    {
        Assert.Equal(5.00m, Sut.Calculate(1000m, PaymentMethod.BankTransfer));
    }

    [Fact]
    public void BankTransferRule_BelowCap_IsOnePercent()
    {
        Assert.Equal(2.50m, Sut.Calculate(250m, PaymentMethod.BankTransfer));
    }

    /// <summary>
    /// The wallet rule was required by the stub but had no test at all.
    /// </summary>
    [Theory]
    [InlineData(200, 3.00)]
    [InlineData(100, 1.50)]
    [InlineData(0, 0)]
    public void WalletRule_IsOnePointFivePercent(double amount, double expected)
    {
        Assert.Equal((decimal)expected, Sut.Calculate((decimal)amount, PaymentMethod.Wallet));
    }

    [Fact]
    public void CardRule_AtZero_StillChargesTheFixedComponent()
    {
        Assert.Equal(0.30m, Sut.Calculate(0m, PaymentMethod.Card));
    }

    /// <summary>
    /// The enum makes the legacy "unknown method" branch unreachable through normal
    /// code, but a cast still gets there. Decide deliberately what should happen.
    /// </summary>
    [Fact]
    public void UndefinedMethod_Throws()
    {
        Assert.ThrowsAny<ArgumentException>(() => Sut.Calculate(100m, (PaymentMethod)99));
    }
}
