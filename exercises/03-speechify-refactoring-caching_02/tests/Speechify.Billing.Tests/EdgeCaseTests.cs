namespace Speechify.Billing.Tests;

/// <summary>
/// Input validation and boundary behaviour for the replacement service.
/// </summary>
/// <remarks>
/// Validation is the ONE place you are allowed to improve on the legacy engine.
/// Everywhere else, <see cref="EquivalenceTests"/> rules. In particular the legacy
/// engine dies with a <see cref="NullReferenceException"/> on a null customer id;
/// here you are required to throw <see cref="ArgumentException"/> instead.
/// </remarks>
public class EdgeCaseTests : LegacyStateIsolated
{
    private static IBillingService Build(FakeRateApi? api = null) =>
        BillingComposition.Create(
            api ?? new FakeRateApi(),
            new FixedTimeProvider(Weekday),
            TimeSpan.FromMinutes(5));

    private static ChargeRequest Valid() =>
        new(100m, "USD", "card", "US", "cust_1", false, 400, null);

    [Fact]
    public void Charge_NullRequest_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => Build().Charge(null!));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Charge_BlankCurrency_Throws(string? currency)
    {
        var request = Valid() with { Currency = currency! };

        Assert.ThrowsAny<ArgumentException>(() => Build().Charge(request));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Charge_BlankMethod_Throws(string? method)
    {
        var request = Valid() with { Method = method! };

        Assert.ThrowsAny<ArgumentException>(() => Build().Charge(request));
    }

    [Theory]
    [InlineData("crypto")]
    [InlineData("cheque")]
    [InlineData("CARD_")]
    public void Charge_UnknownMethod_Throws(string method)
    {
        var request = Valid() with { Method = method };

        Assert.ThrowsAny<ArgumentException>(() => Build().Charge(request));
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-1)]
    [InlineData(-1000)]
    public void Charge_NegativeAmount_Throws(double amount)
    {
        var request = Valid() with { Amount = (decimal)amount };

        Assert.ThrowsAny<ArgumentException>(() => Build().Charge(request));
    }

    [Fact]
    public void Charge_NullCustomerId_ThrowsArgumentException()
    {
        var request = Valid() with { CustomerId = null! };

        // Improvement over the legacy NullReferenceException. This is the single
        // sanctioned behavioural divergence.
        Assert.ThrowsAny<ArgumentException>(() => Build().Charge(request));
    }

    [Fact]
    public void Charge_ZeroAmount_StillChargesTheFixedCardFee()
    {
        ChargeReceipt receipt = Build().Charge(Valid() with { Amount = 0m });

        Assert.Equal(0.30m, receipt.Fee);
        Assert.Equal(0.30m, receipt.TotalUsd);
    }

    [Fact]
    public void Charge_UnsupportedCurrency_FallsBackToParity()
    {
        // Preserved legacy behaviour: a rate lookup failure is swallowed and the
        // charge completes at 1:1 rather than surfacing an error.
        ChargeReceipt receipt = Build().Charge(Valid() with { Currency = "XYZ" });

        Assert.Equal(103.20m, receipt.TotalUsd);
        Assert.Equal("XYZ", receipt.Currency);
    }

    [Fact]
    public void Charge_WalletRule_IsOnePointFivePercent()
    {
        ChargeReceipt receipt = Build().Charge(Valid() with { Amount = 200m, Method = "wallet" });

        Assert.Equal(3.00m, receipt.Fee);
    }

    [Fact]
    public void Charge_BankTransferRule_IsCappedAtFive()
    {
        ChargeReceipt receipt = Build().Charge(Valid() with { Amount = 100_000m, Method = "bank_transfer" });

        Assert.Equal(5m, receipt.Fee);
    }

    [Fact]
    public void EstimateFee_UnknownMethod_Throws()
    {
        Assert.ThrowsAny<ArgumentException>(() => Build().EstimateFee(100m, "crypto"));
    }

    [Fact]
    public void ChargeBatch_NullSequence_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => Build().ChargeBatch(null!));
    }

    [Fact]
    public void ChargeBatch_EmptySequence_ReturnsEmpty()
    {
        Assert.Empty(Build().ChargeBatch([]));
    }

    [Fact]
    public void ChargeBatch_PreservesInputOrder()
    {
        var requests = Enumerable.Range(1, 10)
            .Select(i => Valid() with { Amount = i * 10m })
            .ToList();

        IReadOnlyList<ChargeReceipt> receipts = Build().ChargeBatch(requests);

        Assert.Equal(
            requests.Select(r => r.Amount).ToArray(),
            receipts.Select(r => r.Amount).ToArray());
    }
}
