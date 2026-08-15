using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex03_ListsTests
{
    [Fact]
    public void FilterAbove_ReturnsOnlyItemsGreaterThanThreshold()
    {
        var result = ListExercises.FilterAbove([1, 5, 3, 8, 2], 4);
        Assert.Equal([5, 8], result);
    }

    [Fact]
    public void SortDescending_ReturnsSortedCopyHighestFirst()
    {
        var source = new List<int> { 3, 1, 4, 1, 5 };
        var result = ListExercises.SortDescending(source);
        Assert.Equal([5, 4, 3, 1, 1], result);
        Assert.Equal([3, 1, 4, 1, 5], source); // original unchanged
    }

    [Fact]
    public void RemoveDuplicates_PreservesFirstOccurrenceOrder()
    {
        var result = ListExercises.RemoveDuplicates([1, 2, 1, 3, 2, 4]);
        Assert.Equal([1, 2, 3, 4], result);
    }

    [Fact]
    public void ElementsAtEvenIndices_ReturnsIndex0_2_4()
    {
        var result = ListExercises.ElementsAtEvenIndices(["a","b","c","d","e"]);
        Assert.Equal(["a","c","e"], result);
    }

    [Fact]
    public void RotateLeft_By2_ShiftsCorrectly()
    {
        var result = ListExercises.RotateLeft([1,2,3,4,5], 2);
        Assert.Equal([3,4,5,1,2], result);
    }
}
