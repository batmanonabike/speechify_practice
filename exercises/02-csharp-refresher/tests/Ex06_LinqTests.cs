using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex06_LinqFilteringTests
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
        var result = LinqFilteringExercises.ProductNamesInCategory(Lines, "Fruit").ToList();
        Assert.Equal(["Apple", "Banana"], result);
    }

    [Fact]
    public void TopByPrice_ReturnsTopN()
    {
        var result = LinqFilteringExercises.TopByPrice(Lines, 2).ToList();
        Assert.Equal("Diesel", result[0].Product);
        Assert.Equal("Apple",  result[1].Product);
    }

    [Fact]
    public void HighValueLines_ReturnsLinesAboveThreshold()
    {
        // Diesel: 90*2=180, Carrot: 0.8*15=12, Apple: 1*10=10, Banana: 0.5*20=10
        var result = LinqFilteringExercises.HighValueLines(Lines, 11m).ToList();
        Assert.Equal(2, result.Count);
        Assert.Contains(result, l => l.Product == "Diesel");
        Assert.Contains(result, l => l.Product == "Carrot");
    }
}

