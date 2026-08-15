using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex19_DelegatesTests
{
    [Fact]
    public void Map_TransformsAllElements()
    {
        var result = DelegateExercises.Map([1,2,3], x => x * 2).ToList();
        Assert.Equal([2,4,6], result);
    }

    [Fact]
    public void Compose_ChainsCorrectly()
    {
        Func<int,string> fn = DelegateExercises.Compose<int,int,string>(
            x => x + 1, x => x.ToString());
        Assert.Equal("6", fn(5));
    }

    [Fact]
    public void Repeat_CallsActionCorrectTimes()
    {
        var calls = new List<int>();
        DelegateExercises.Repeat(4, i => calls.Add(i));
        Assert.Equal([0,1,2,3], calls);
    }

    [Fact]
    public void Memoize_CallsFnOnlyOnce()
    {
        int callCount = 0;
        var memo = DelegateExercises.Memoize<int,int>(x => { callCount++; return x * 10; });
        Assert.Equal(50, memo(5));
        Assert.Equal(50, memo(5));
        Assert.Equal(1, callCount);
    }

    [Fact]
    public void StockTicker_RaisesEvent_OnPriceChange()
    {
        var ticker = new StockTicker();
        double? received = null;
        ticker.PriceChanged += (_, p) => received = p;
        ticker.Price = 100.0;
        Assert.Equal(100.0, received);
    }

    [Fact]
    public void StockTicker_DoesNotRaiseEvent_WhenPriceUnchanged()
    {
        var ticker = new StockTicker();
        int count  = 0;
        ticker.PriceChanged += (_, _) => count++;
        ticker.Price = 50;
        ticker.Price = 50; // same value
        Assert.Equal(1, count);
    }
}
