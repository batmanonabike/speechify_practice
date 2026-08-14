using System.Collections.Immutable;
using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex05_ImmutableTests
{
    [Fact]
    public void RemoveNegatives_ReturnsNewListWithoutNegatives()
    {
        var src    = ImmutableList.Create(1, -2, 3, -4, 5);
        var result = ImmutableExercises.RemoveNegatives(src);
        Assert.Equal([1, 3, 5], result);
    }

    [Fact]
    public void ApplyUpdates_ReturnsUpdatedDictionary()
    {
        var dict    = ImmutableDictionary.CreateRange(new[] {
            KeyValuePair.Create("a", 1), KeyValuePair.Create("b", 2) });
        var updates = new Dictionary<string,int> { ["b"] = 99, ["c"] = 3 };
        var result  = ImmutableExercises.ApplyUpdates(dict, updates);
        Assert.Equal(1,  result["a"]);
        Assert.Equal(99, result["b"]);
        Assert.Equal(3,  result["c"]);
    }

    [Fact]
    public void SquaresUpTo_ReturnsImmutableArrayOfSquares()
    {
        var result = ImmutableExercises.SquaresUpTo(5);
        Assert.Equal([1, 4, 9, 16, 25], result);
    }
}
