using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex08_LinqJoinsTests
{
    [Fact]
    public void WithIndex_AttachesCorrectIndices()
    {
        var result = LinqJoinExercises.WithIndex(new[] { "x","y","z" }).ToList();
        Assert.Equal(("x", 0), result[0]);
        Assert.Equal(("y", 1), result[1]);
        Assert.Equal(("z", 2), result[2]);
    }

    [Fact]
    public void ApplyDiscounts_MatchesDiscountRate()
    {
        var lines     = new[] { new OrderLine("Apple","Fruit",1m,1), new OrderLine("Banana","Fruit",0.5m,1) };
        var discounts = new[] { ("Apple", 0.10m) };
        var result    = LinqJoinExercises.ApplyDiscounts(lines, discounts).ToList();
        Assert.Equal(0.10m, result.First(r => r.Line.Product == "Apple").DiscountRate);
        Assert.Equal(0.00m, result.First(r => r.Line.Product == "Banana").DiscountRate);
    }

    [Fact]
    public void InnerJoin_ReturnsPairedMatches()
    {
        var left  = new[] { (Id: 1, Name: "Alice"), (Id: 2, Name: "Bob"), (Id: 3, Name: "Carol") };
        var right = new[] { (Id: 1, Score: 90),     (Id: 3, Score: 75) };
        var result = LinqJoinExercises.InnerJoin(left, right, l => l.Id, r => r.Id).ToList();
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.Left.Name == "Alice" && r.Right.Score == 90);
        Assert.Contains(result, r => r.Left.Name == "Carol" && r.Right.Score == 75);
    }
}
