using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex26_IEquatableTests
{
    [Fact]
    public void Money_EqualWhenCurrencyAndAmountMatch()
    {
        var a = new Money("GBP", 10.00m);
        var b = new Money("GBP", 10.00m);
        Assert.Equal(a, b);
        Assert.True(a == b);
        Assert.False(a != b);
    }

    [Fact]
    public void Money_NotEqualWhenCurrencyDiffers()
    {
        var a = new Money("GBP", 10.00m);
        var b = new Money("USD", 10.00m);
        Assert.NotEqual(a, b);
        Assert.True(a != b);
    }

    [Fact]
    public void Money_GetHashCode_SameForEqualInstances()
    {
        var a = new Money("EUR", 5.00m);
        var b = new Money("EUR", 5.00m);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

}
