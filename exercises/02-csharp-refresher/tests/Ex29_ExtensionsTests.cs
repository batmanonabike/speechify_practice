using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex29_ExtensionsTests
{
    [Theory]
    [InlineData("hello world", "Hello World")]
    [InlineData("the quick brown fox", "The Quick Brown Fox")]
    public void ToTitleCase_CapitalisesEachWord(string input, string expected)
        => Assert.Equal(expected, input.ToTitleCase());

    [Theory]
    [InlineData("42",   true)]
    [InlineData("-7",   true)]
    [InlineData("3.14", false)]
    [InlineData("abc",  false)]
    public void IsInteger_DetectsCorrectly(string s, bool expected)
        => Assert.Equal(expected, s.IsInteger());

    [Fact]
    public void OrEmpty_NullSource_ReturnsEmptySequence()
    {
        IEnumerable<int>? src = null;
        Assert.Empty(src.OrEmpty());
    }

    [Fact]
    public void Partition_SplitsCorrectly()
    {
        var (evens, odds) = new[] {1,2,3,4,5}.Partition(x => x % 2 == 0);
        Assert.Equal([2,4], evens);
        Assert.Equal([1,3,5], odds);
    }

    [Fact]
    public void DictionaryGetOrAdd_AddsAndReturnsNewValue()
    {
        var dict = new Dictionary<string,int>();
        var v    = dict.GetOrAdd("key", k => 99);
        Assert.Equal(99, v);
        Assert.Equal(99, dict["key"]);
    }

    [Fact]
    public void DictionaryGetOrAdd_ReturnsExistingValue()
    {
        var dict = new Dictionary<string,int> { ["key"] = 1 };
        var v    = dict.GetOrAdd("key", k => 999);
        Assert.Equal(1, v);
    }
}
