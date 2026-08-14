using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex15_StringsTests
{
    [Theory]
    [InlineData("MyVariableName", "my_variable_name")]
    [InlineData("helloWorld",     "hello_world")]
    [InlineData("ABC",            "a_b_c")]
    public void ToSnakeCase_ConvertsCorrectly(string input, string expected)
        => Assert.Equal(expected, StringExercises.ToSnakeCase(input));

    [Fact]
    public void CountOccurrences_CountsNonOverlapping()
    {
        Assert.Equal(3, StringExercises.CountOccurrences("ababab", "ab"));
        Assert.Equal(0, StringExercises.CountOccurrences("hello",  "xyz"));
    }

    [Fact]
    public void Truncate_ShorterThanMax_Unchanged()
        => Assert.Equal("hi", StringExercises.Truncate("hi", 10));

    [Fact]
    public void Truncate_LongerThanMax_AddsEllipsis()
    {
        var result = StringExercises.Truncate("hello world", 5);
        Assert.StartsWith("hello", result);
        Assert.EndsWith("…", result);
    }

    [Fact]
    public void ReverseWords_ReversesWordOrder()
        => Assert.Equal("foo world hello", StringExercises.ReverseWords("hello world foo"));

    [Theory]
    [InlineData("listen", "silent", true)]
    [InlineData("hello",  "world",  false)]
    [InlineData("Astronomer", "Moon starer", true)]
    public void IsAnagram_DetectsCorrectly(string s, string t, bool expected)
        => Assert.Equal(expected, StringExercises.IsAnagram(s, t));

    [Fact]
    public void CsvOfRange_GeneratesCommaSeparatedList()
        => Assert.Equal("1,2,3,4,5", StringExercises.CsvOfRange(5));
}
