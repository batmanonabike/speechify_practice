using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex25_ValueEqualityTests
{
    // --- PriceClass ---
    [Fact]
    public void PriceClass_EqualWhenBothFieldsMatch()
    {
        var a = new PriceClass("GBP", 9.99m);
        var b = new PriceClass("GBP", 9.99m);
        Assert.Equal(a, b);
        Assert.True(a == b);
    }

    [Fact]
    public void PriceClass_NotEqualWhenAmountDiffers()
    {
        var a = new PriceClass("GBP", 1m);
        var b = new PriceClass("GBP", 2m);
        Assert.NotEqual(a, b);
    }

    // --- PriceRecord ---
    [Fact]
    public void PriceRecord_RecordEquality_Works()
    {
        var a = new PriceRecord("USD", 5.00m);
        var b = new PriceRecord("USD", 5.00m);
        Assert.Equal(a, b);
    }

    [Fact]
    public void PriceRecord_Discounted_ReturnsNewRecordWithReducedAmount()
    {
        var a = new PriceRecord("USD", 100m);
        var b = a.Discounted(10); // 10% off
        Assert.Equal(90m, b.Amount);
        Assert.Equal(100m, a.Amount); // original unchanged
    }

    // --- PriceStruct ---
    [Fact]
    public void PriceStruct_StructEquality_Works()
    {
        var a = new PriceStruct { Currency = "EUR", Amount = 3.50m };
        var b = new PriceStruct { Currency = "EUR", Amount = 3.50m };
        Assert.True(a.Equals(b));
    }

    // --- PriceRecordStruct ---
    [Fact]
    public void PriceRecordStruct_RecordStructEquality_Works()
    {
        var a = new PriceRecordStruct("JPY", 1000m);
        var b = new PriceRecordStruct("JPY", 1000m);
        Assert.Equal(a, b);
    }

    [Fact]
    public void PriceRecordStruct_WithTax_ReturnsNewWithTax()
    {
        var a = new PriceRecordStruct("JPY", 1000m);
        var b = a.WithTax(0.1m); // 10% tax
        Assert.Equal(1100m, b.Amount);
    }
}
