using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex28_ArraysTests
{
    [Fact]
    public void SecondLargest_ReturnsCorrectValue()
        => Assert.Equal(4, ArrayExercises.SecondLargest([3,1,4,1,5,9,2,6]));

    [Fact]
    public void SecondLargest_AllSame_Throws()
        => Assert.Throws<InvalidOperationException>(() => ArrayExercises.SecondLargest([2,2,2]));

    [Fact]
    public void RotateMatrix90_RotatesClockwise()
    {
        var m = new int[,] { {1,2,3},{4,5,6},{7,8,9} };
        ArrayExercises.RotateMatrix90(m);
        Assert.Equal(7, m[0,0]);
        Assert.Equal(4, m[0,1]);
        Assert.Equal(1, m[0,2]);
        Assert.Equal(8, m[1,0]);
        Assert.Equal(9, m[2,0]);
    }

    [Fact]
    public void CountChar_CountsWithoutAllocation()
    {
        Assert.Equal(3, ArrayExercises.CountChar("banana".AsSpan(), 'a'));
        Assert.Equal(0, ArrayExercises.CountChar("hello".AsSpan(),  'z'));
    }

    [Fact]
    public void PageSize_IsPositive()
        => Assert.True(ArrayExercises.PageSize > 0);

    [Fact]
    public void DefaultTimeout_IsPositive()
        => Assert.True(ArrayExercises.DefaultTimeout > TimeSpan.Zero);
}
