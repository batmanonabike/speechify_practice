using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex22_RecordsVsClassesTests
{
    [Fact]
    public void ProductRecord_ValueEquality_Works()
    {
        var a = new ProductRecord("P1", "Apple", 1.50m);
        var b = new ProductRecord("P1", "Apple", 1.50m);
        Assert.Equal(a, b);
        Assert.True(a == b);
    }

    [Fact]
    public void ProductRecord_WithExpression_CreatesNewWithChangedPrice()
    {
        var a = new ProductRecord("P1", "Apple", 1.50m);
        var b = a with { Price = 2.00m };
        Assert.Equal(1.50m, a.Price); // original unchanged
        Assert.Equal(2.00m, b.Price);
    }

    [Fact]
    public void ProductClass_EqualById_DifferentObjects()
    {
        var a = new ProductClass("P1", "Apple", 1.50m);
        var b = new ProductClass("P1", "Banana", 9.99m); // same Id, different data
        Assert.Equal(a, b);
    }

    [Fact]
    public void ProductClass_NotEqual_DifferentId()
    {
        var a = new ProductClass("P1", "Apple", 1.50m);
        var b = new ProductClass("P2", "Apple", 1.50m);
        Assert.NotEqual(a, b);
    }
}
