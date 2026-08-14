using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex03_SetsTests
{
    [Fact]
    public void Intersection_ReturnsCommonElements()
    {
        var result = SetExercises.Intersection([1,2,3,4], [3,4,5,6]);
        Assert.Equal([3,4], result.Order());
    }

    [Fact]
    public void SymmetricDifference_ReturnsElementsInEitherNotBoth()
    {
        var result = SetExercises.SymmetricDifference([1,2,3], [2,3,4]);
        Assert.Equal([1,4], result.Order());
    }

    [Fact]
    public void IsProperSubset_ReturnsTrue_WhenSubset()
    {
        Assert.True(SetExercises.IsProperSubset([1,2], [1,2,3]));
        Assert.False(SetExercises.IsProperSubset([1,2,3], [1,2,3]));
    }

    [Fact]
    public void FindDuplicates_ReturnsDuplicatedValues()
    {
        var result = SetExercises.FindDuplicates([1,2,2,3,3,3,4]);
        Assert.Equal([2,3], result.Order());
    }
}
