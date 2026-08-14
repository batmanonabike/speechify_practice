using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex02_DictionariesTests
{
    [Fact]
    public void WordFrequency_CountsCorrectly()
    {
        var result = DictionaryExercises.WordFrequency(["a","b","A","c","b"]);
        Assert.Equal(2, result["a"]);
        Assert.Equal(2, result["b"]);
        Assert.Equal(1, result["c"]);
    }

    [Fact]
    public void Invert_SwapsKeysAndValues()
    {
        var dict = new Dictionary<string,int> { ["one"]=1, ["two"]=2 };
        var inv  = DictionaryExercises.Invert(dict);
        Assert.Equal("one", inv[1]);
        Assert.Equal("two", inv[2]);
    }

    [Fact]
    public void Merge_SecondWins_OnConflict()
    {
        var a = new Dictionary<string,int> { ["x"]=1, ["y"]=2 };
        var b = new Dictionary<string,int> { ["y"]=99, ["z"]=3 };
        var m = DictionaryExercises.Merge(a, b);
        Assert.Equal(1,  m["x"]);
        Assert.Equal(99, m["y"]);
        Assert.Equal(3,  m["z"]);
    }

    [Fact]
    public void GroupBy_GroupsItemsByKey()
    {
        var items  = new[] { 1, 2, 3, 4, 5, 6 };
        var groups = DictionaryExercises.GroupBy(items, x => x % 2 == 0 ? "even" : "odd");
        Assert.Equal([2,4,6], groups["even"].Order());
        Assert.Equal([1,3,5], groups["odd"].Order());
    }
}
