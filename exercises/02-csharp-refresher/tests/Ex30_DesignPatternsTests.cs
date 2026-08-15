using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex30_DesignPatternsTests
{
    // Strategy
    [Fact]
    public void AscendingSort_SortsLowToHigh()
    {
        var sorter = new Sorter<int>(new AscendingSort<int>());
        Assert.Equal([1,2,3,4], sorter.Sort([3,1,4,2]));
    }

    [Fact]
    public void DescendingSort_SortsHighToLow()
    {
        var sorter = new Sorter<int>(new DescendingSort<int>());
        Assert.Equal([4,3,2,1], sorter.Sort([3,1,4,2]));
    }

    [Fact]
    public void Sorter_SetStrategy_ChangesSort()
    {
        var sorter = new Sorter<int>(new AscendingSort<int>());
        sorter.SetStrategy(new DescendingSort<int>());
        Assert.Equal([3,2,1], sorter.Sort([1,2,3]));
    }

    // Decorator
    [Fact]
    public void TimestampedSender_PrependTimestamp()
    {
        var sender = new TimestampedSender(new ConsoleSender());
        var result = sender.Send("hello");
        Assert.Contains("SENT: hello", result);
        // timestamp bracket should appear before the inner result
        Assert.StartsWith("[", result);
    }

    // Factory
    [Fact]
    public void ShapeFactory_CreateSquare_CorrectArea()
    {
        var shape = ShapeFactory.Create("square", 4, 0);
        Assert.Equal(16, shape.Area());
    }

    [Fact]
    public void ShapeFactory_CreateTriangle_CorrectArea()
    {
        var shape = ShapeFactory.Create("triangle", 6, 4);
        Assert.Equal(12, shape.Area());
    }

    [Fact]
    public void ShapeFactory_UnknownType_ThrowsArgumentException()
        => Assert.Throws<ArgumentException>(() => ShapeFactory.Create("hexagon", 1, 1));
}
