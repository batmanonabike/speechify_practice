using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex09_LinqDeferredTests
{
    [Fact]
    public void Batch_SplitsIntoCorrectChunks()
    {
        var result = LinqDeferredExercises.Batch(Enumerable.Range(1, 7), 3).ToList();
        Assert.Equal(3, result.Count);
        Assert.Equal([1,2,3], result[0]);
        Assert.Equal([4,5,6], result[1]);
        Assert.Equal([7],     result[2]);
    }

    [Fact]
    public void LazyEvenTripled_ReturnsCorrectValues()
    {
        var result = LinqDeferredExercises.LazyEvenTripled(Enumerable.Range(1, 10), 3).ToList();
        Assert.Equal([6, 12, 18], result); // evens: 2,4,6 → *3
    }

    [Fact]
    public void DeferredExecutionDemo_ReturnsTrue()
        => Assert.True(LinqDeferredExercises.DeferredExecutionDemo());
}
