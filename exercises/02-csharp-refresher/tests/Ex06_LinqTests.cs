using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex06_LinqTests
{
    private static readonly OrderLine[] Lines =
    [
        new("Apple",  "Fruit",  1.00m, 10),
        new("Banana", "Fruit",  0.50m, 20),
        new("Carrot", "Veggie", 0.80m, 15),
        new("Diesel", "Fuel",  90.00m,  2),
    ];

    [Fact]
    public void ProductNamesInCategory_ReturnsSortedNames()
    {
        var result = LinqExercises.ProductNamesInCategory(Lines, "Fruit").ToList();
        Assert.Equal(["Apple", "Banana"], result);
    }

    [Fact]
    public void TopByPrice_ReturnsTopN()
    {
        var result = LinqExercises.TopByPrice(Lines, 2).ToList();
        Assert.Equal("Diesel", result[0].Product);
        Assert.Equal("Apple",  result[1].Product);
    }

    [Fact]
    public void RevenueByCategory_SumsCorrectly()
    {
        var result = LinqExercises.RevenueByCategory(Lines);
        Assert.Equal(20.00m, result["Fruit"]);   // 1*10 + 0.5*20
        Assert.Equal(12.00m, result["Veggie"]);  // 0.8*15
        Assert.Equal(180.00m, result["Fuel"]);   // 90*2
    }

    [Fact]
    public void HighestRevenueItem_ReturnsDiesel()
    {
        var result = LinqExercises.HighestRevenueItem(Lines);
        Assert.Equal("Diesel", result.Product);
    }

    [Fact]
    public void WithIndex_AttachesCorrectIndices()
    {
        var result = LinqExercises.WithIndex(new[] { "x","y","z" }).ToList();
        Assert.Equal(("x", 0), result[0]);
        Assert.Equal(("z", 2), result[2]);
    }

    [Fact]
    public void Batch_SplitsIntoChunks()
    {
        var result = LinqExercises.Batch(Enumerable.Range(1, 7), 3).ToList();
        Assert.Equal(3, result.Count);
        Assert.Equal([1,2,3], result[0]);
        Assert.Equal([7],     result[2]);
    }
}
