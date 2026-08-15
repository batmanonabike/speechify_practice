using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex18_GenericsTests
{
    [Fact]
    public void BoundedStack_PushPop_Works()
    {
        var s = new BoundedStack<int>(3);
        s.Push(1); s.Push(2); s.Push(3);
        Assert.Equal(3, s.Count);
        Assert.Equal(3, s.Pop());
        Assert.Equal(2, s.Peek());
    }

    [Fact]
    public void BoundedStack_Push_WhenFull_Throws()
    {
        var s = new BoundedStack<int>(1);
        s.Push(1);
        Assert.Throws<InvalidOperationException>(() => s.Push(2));
    }

    [Fact]
    public void BoundedStack_Pop_WhenEmpty_Throws()
        => Assert.Throws<InvalidOperationException>(() => new BoundedStack<int>(2).Pop());

    [Fact]
    public void GenericUtils_Max_ReturnsLarger()
        => Assert.Equal(7, GenericUtils.Max(3, 7));

    [Fact]
    public void GenericUtils_Coalesce_ReturnsFallbackWhenNull()
        => Assert.Equal("default", GenericUtils.Coalesce<string>(null, "default"));

    [Fact]
    public void GenericUtils_DistinctBy_KeepsFirstOccurrence()
    {
        var items  = new[] { "apple","apricot","banana","blueberry" };
        var result = GenericUtils.DistinctBy(items, s => s[0]).ToList();
        Assert.Equal(["apple","banana"], result);
    }
}
