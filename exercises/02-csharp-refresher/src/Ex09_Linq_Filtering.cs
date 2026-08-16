// Exercise 09 - LINQ: Filtering & projection
// Reference: docs/csharp-refresher/09_Linq_FilterProjectOrder.cs

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
    {
        return lines
            .Where(x => String.Equals(x.Category, category))
            .OrderBy(x => x.Product)
            .Select(x => x.Product);
    }

    /// <summary>
    /// Return the top <paramref name="n"/> most expensive lines (by Price descending).
    /// Hint: OrderByDescending + Take.
    /// </summary>
    public static IEnumerable<OrderLine> TopByPrice(IEnumerable<OrderLine> lines, int n)
        => lines.OrderByDescending(x => x.Price).Take(n);

    /// <summary>
    /// Return all lines where Price * Quantity exceeds <paramref name="threshold"/>.
    /// Hint: Where.
    /// </summary>
    public static IEnumerable<OrderLine> HighValueLines(
        IEnumerable<OrderLine> lines, decimal threshold)
    {
        return lines.Where(x => x.Price * x.Quantity > threshold);
    }
}

