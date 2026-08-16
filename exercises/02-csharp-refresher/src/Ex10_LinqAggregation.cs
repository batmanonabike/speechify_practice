// Exercise 10 - LINQ: Aggregation & grouping
// Reference: docs/csharp-refresher/10_Linq_AggregateGroup.cs

namespace CSharpExercises;

public static class LinqAggregationExercises
{
    /// <summary>
    /// Return total revenue (Price * Quantity) per category.
    /// Hint: GroupBy + Sum.
    /// </summary>
    public static Dictionary<string, decimal> RevenueByCategory(IEnumerable<OrderLine> lines)
    {
        var groupedLines = lines.GroupBy(b => b.Category);
        return groupedLines.ToDictionary(
            a => a.Key,
            a => a.Sum(line => line.Price * line.Quantity));
    }

    /// <summary>
    /// Return the single OrderLine with the highest total revenue (Price * Quantity).
    /// Throw InvalidOperationException if the collection is empty.
    /// Hint: MaxBy.
    /// </summary>
    public static OrderLine HighestRevenueItem(IEnumerable<OrderLine> lines)
    {
        return lines.MaxBy(x => x.Price * x.Quantity) ?? throw new InvalidOperationException();
    }

    /// <summary>
    /// Return the average price per category as a dictionary.
    /// Hint: GroupBy + Average.
    /// </summary>
    public static Dictionary<string, double> AveragePriceByCategory(IEnumerable<OrderLine> lines)
    {
        var groupedLines = lines.GroupBy(b => b.Category);
        return groupedLines.ToDictionary(
            a => a.Key,
            a => a.Average(line => Convert.ToDouble(line.Price)));
    }
}
