using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex30_IEquatableTests
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

    [Fact]
    public void SemanticVersion_CompareTo_SortsCorrectly()
    {
        var v100 = new SemanticVersion(1,0,0);
        var v110 = new SemanticVersion(1,1,0);
        var v111 = new SemanticVersion(1,1,1);
        Assert.True(v100 < v110);
        Assert.True(v110 < v111);
        Assert.True(v111 > v100);
    }

    [Fact]
    public void SemanticVersion_List_SortsAscending()
    {
        var versions = new[]
        {
            new SemanticVersion(2,0,0),
            new SemanticVersion(1,0,0),
            new SemanticVersion(1,1,0),
        };
        Array.Sort(versions);
        Assert.Equal(new SemanticVersion(1,0,0), versions[0]);
        Assert.Equal(new SemanticVersion(2,0,0), versions[2]);
    }
}
