using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex10_LinqAggregationTests
{
    private static readonly OrderLine[] Lines =
    [
        new("Apple",  "Fruit",  1.00m, 10),
        new("Banana", "Fruit",  0.50m, 20),
        new("Carrot", "Veggie", 0.80m, 15),
        new("Diesel", "Fuel",  90.00m,  2),
    ];

    [Fact]
    public void RevenueByCategory_SumsCorrectly()
    {
        var result = LinqAggregationExercises.RevenueByCategory(Lines);
        Assert.Equal(20.00m,  result["Fruit"]);   // 1*10 + 0.5*20
        Assert.Equal(12.00m,  result["Veggie"]);  // 0.8*15
        Assert.Equal(180.00m, result["Fuel"]);    // 90*2
    }

    [Fact]
    public void HighestRevenueItem_ReturnsDiesel()
    {
        var result = LinqAggregationExercises.HighestRevenueItem(Lines);
        Assert.Equal("Diesel", result.Product);
    }

    [Fact]
    public void HighestRevenueItem_EmptyCollection_Throws()
        => Assert.Throws<InvalidOperationException>(
            () => LinqAggregationExercises.HighestRevenueItem([]));

    [Fact]
    public void AveragePriceByCategory_ReturnsCorrectAverages()
    {
        var result = LinqAggregationExercises.AveragePriceByCategory(Lines);
        Assert.Equal(0.75, result["Fruit"],  5); // (1.00+0.50)/2
        Assert.Equal(0.80, result["Veggie"], 5);
    }
}
