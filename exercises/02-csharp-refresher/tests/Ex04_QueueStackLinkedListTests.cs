using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex04_QueueStackLinkedListTests
{
    [Fact]
    public void ReverseString_ReversesCharacters()
    {
        Assert.Equal("olleh", QueueStackExercises.ReverseString("hello"));
    }

    [Fact]
    public void IsPalindrome_DetectsCorrectly()
    {
        Assert.True(QueueStackExercises.IsPalindrome("racecar"));
        Assert.False(QueueStackExercises.IsPalindrome("hello"));
    }

    [Fact]
    public void ProcessQueue_ReturnsItemsInOrder()
    {
        var result = QueueStackExercises.ProcessQueue(["task1", "task2", "task3"]);
        Assert.Equal(["task1", "task2", "task3"], result);
    }

    [Fact]
    public void RemoveNegatives_FiltersNegativeNodes()
    {
        var list   = new LinkedList<int>(new[] { 1, -2, 3, -4, 5 });
        var result = QueueStackExercises.RemoveNegatives(list);
        Assert.Equal([1, 3, 5], result);
    }
}
