// Exercise 06 — LINQ: Filtering & projection
// Reference: docs/csharp-refresher/06_Linq_Filtering.cs

namespace CSharpExercises;

public record OrderLine(string Product, string Category, decimal Price, int Quantity);

public static class LinqFilteringExercises
{
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

    /// <summary>
    /// Return all lines where Price * Quantity exceeds <paramref name="threshold"/>.
    /// Hint: Where.
    /// </summary>
    public static IEnumerable<OrderLine> HighValueLines(
        IEnumerable<OrderLine> lines, decimal threshold)
        => throw new NotImplementedException();
}

