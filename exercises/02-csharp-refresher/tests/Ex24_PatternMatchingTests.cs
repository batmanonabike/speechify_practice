using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex24_PatternMatchingTests
{
    [Fact]
    public void Area_Circle_IsCorrect()
        => Assert.Equal(Math.PI * 4, PatternMatchingExercises.Area(new Circle2D(2)), 6);

    [Fact]
    public void Area_Rectangle_IsCorrect()
        => Assert.Equal(12, PatternMatchingExercises.Area(new Rectangle2D(3,4)));

    [Fact]
    public void Area_Triangle_UsesHeron()
    {
        // 3-4-5 right triangle, area = 6
        var area = PatternMatchingExercises.Area(new Triangle2D(3, 4, 5));
        Assert.Equal(6, area, 6);
    }

    [Theory]
    [InlineData(-5,  "negative")]
    [InlineData(0,   "zero")]
    [InlineData(5,   "small")]
    [InlineData(100, "large")]
    public void Classify_ReturnsCorrectLabel(int n, string expected)
        => Assert.Equal(expected, PatternMatchingExercises.Classify(n));

    [Theory]
    [InlineData(null,    "null")]
    [InlineData("",      "empty")]
    [InlineData("hi",    "short")]
    [InlineData("hello world", "long")]
    public void Describe_ReturnsCorrectDescription(string? s, string expected)
        => Assert.Equal(expected, PatternMatchingExercises.Describe(s));

    [Fact]
    public void DecodeCommand_Move_ParsesCoordinates()
        => Assert.Equal("Move to (10,20)", PatternMatchingExercises.DecodeCommand(["MOVE","10","20"]));

    [Fact]
    public void DecodeCommand_Fire_ReturnsFire()
        => Assert.Equal("Fire!", PatternMatchingExercises.DecodeCommand(["FIRE"]));

    [Fact]
    public void DecodeCommand_Empty_ReturnsEmpty()
        => Assert.Equal("Empty", PatternMatchingExercises.DecodeCommand([]));
}
