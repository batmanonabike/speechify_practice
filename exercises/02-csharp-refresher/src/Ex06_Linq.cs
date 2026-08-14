// Exercises 06–09 — LINQ
// Reference: docs/csharp-refresher/06–09_Linq_*.cs

namespace CSharpExercises;

public record OrderLine(string Product, string Category, decimal Price, int Quantity);

public static class LinqExercises
{
    // ---- Filtering & projection (06) ----

    /// <summary>
    /// Return the names of all products in <paramref name="category"/>, sorted A→Z.
    /// Hint: Where + OrderBy + Select.
    /// </summary>
    public static IEnumerable<string> ProductNamesInCategory(
        IEnumerable<OrderLine> lines, string category)
        => throw new NotImplementedException();

    /// <summary>
    /// Return the top <paramref name="n"/> most expensive lines (by Price descending).
    /// Hint: OrderByDescending + Take.
    /// </summary>
    public static IEnumerable<OrderLine> TopByPrice(IEnumerable<OrderLine> lines, int n)
        => throw new NotImplementedException();

    // ---- Aggregation (07) ----

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

    // ---- Joins & zipping (08) ----

    /// <summary>
    /// Given two sequences of equal length, return a sequence of (item, index) tuples.
    /// Hint: Select with index overload, or Zip with Enumerable.Range.
    /// </summary>
    public static IEnumerable<(T Item, int Index)> WithIndex<T>(IEnumerable<T> source)
        => throw new NotImplementedException();

    /// <summary>
    /// Left-join <paramref name="lines"/> to <paramref name="discounts"/> on Product name.
    /// Return each line with its discount rate (0 if not found).
    /// Hint: GroupJoin + SelectMany with DefaultIfEmpty.
    /// </summary>
    public static IEnumerable<(OrderLine Line, decimal DiscountRate)> ApplyDiscounts(
        IEnumerable<OrderLine> lines,
        IEnumerable<(string Product, decimal Rate)> discounts)
        => throw new NotImplementedException();

    // ---- Deferred execution (09) ----

    /// <summary>
    /// Split <paramref name="source"/> into batches of <paramref name="size"/>.
    /// Hint: Chunk (NET 6+) or manual batch logic with Skip/Take.
    /// </summary>
    public static IEnumerable<IEnumerable<T>> Batch<T>(IEnumerable<T> source, int size)
        => throw new NotImplementedException();
}
