// Exercise 07 — LINQ: Aggregation & grouping
// Reference: docs/csharp-refresher/07_Linq_Aggregation.cs

namespace CSharpExercises;

public static class LinqAggregationExercises
{
    /// <summary>
    /// Return total revenue (Price * Quantity) per category.
    /// Hint: GroupBy + Sum.
    /// </summary>
    public static Dictionary<string, decimal> RevenueByCategory(IEnumerable<OrderLine> lines)
        => throw new NotImplementedException();

    /// <summary>
    /// Return the single OrderLine with the highest total revenue (Price * Quantity).
    /// Throw InvalidOperationException if the collection is empty.
    /// Hint: MaxBy.
    /// </summary>
    public static OrderLine HighestRevenueItem(IEnumerable<OrderLine> lines)
        => throw new NotImplementedException();

    /// <summary>
    /// Return the average price per category as a dictionary.
    /// Hint: GroupBy + Average.
    /// </summary>
    public static Dictionary<string, double> AveragePriceByCategory(IEnumerable<OrderLine> lines)
        => throw new NotImplementedException();
}
