using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex08_IndexRangeSpreadTests
{
    [Fact]
    public void Last_ReturnsFinalElement()
        => Assert.Equal("Sun", IndexRangeExercises.Last(["Mon", "Tue", "Sun"]));

    [Fact]
    public void TakeLast_ReturnsNewArrayWithRequestedSuffix()
    {
        var source = new[] { 10, 20, 30, 40, 50 };
        var result = IndexRangeExercises.TakeLast(source, 2);

        Assert.Equal([40, 50], result);
        Assert.NotSame(source, result);
    }

    [Fact]
    public void WithoutFirstAndLast_ReturnsMiddle()
        => Assert.Equal(["b", "c"], IndexRangeExercises.WithoutFirstAndLast(["a", "b", "c", "d"]));

    [Fact]
    public void WithoutFirstAndLast_ShortInputReturnsEmpty()
    {
        Assert.Empty(IndexRangeExercises.WithoutFirstAndLast(Array.Empty<int>()));
        Assert.Empty(IndexRangeExercises.WithoutFirstAndLast([1]));
    }

    [Fact]
    public void Append_UsesCopiesAndDoesNotMutateInputs()
    {
        var source = new[] { 1, 2 };
        var additions = new[] { 3, 4 };

        var result = IndexRangeExercises.Append(source, additions);

        Assert.Equal([1, 2, 3, 4], result);
        Assert.Equal([1, 2], source);
        Assert.Equal([3, 4], additions);
        Assert.NotSame(source, result);
        Assert.NotSame(additions, result);
    }
}
