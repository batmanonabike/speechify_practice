using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex27_ConcurrentTests
{
    [Fact]
    public async Task ParallelCounterAsync_Returns10000()
    {
        var total = await ConcurrentExercises.ParallelCounterAsync();
        Assert.Equal(10_000, total);
    }

    [Fact]
    public async Task ProducerConsumerSumAsync_SumsRange()
    {
        int count = 100;
        var sum   = await ConcurrentExercises.ProducerConsumerSumAsync(count);
        Assert.Equal((long)count * (count - 1) / 2, sum); // 0+1+...+99
    }

    [Fact]
    public void ConcurrentWordFrequency_CountsWords()
    {
        var words  = new[] { "a","b","a","c","b","a" };
        var result = ConcurrentExercises.ConcurrentWordFrequency(words);
        Assert.Equal(3, result["a"]);
        Assert.Equal(2, result["b"]);
        Assert.Equal(1, result["c"]);
    }
}
