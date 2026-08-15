// ============================================================
// LINQ Refresher — Part 1: Filtering, Projecting, Ordering
// ============================================================
// All LINQ methods live in System.Linq.
// Most return IEnumerable<T> and are lazily evaluated —
// nothing executes until you iterate or call a terminal op
// like ToList(), ToArray(), First(), Count(), etc.
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace CollectionsRefresher;

public record Product(string Name, string Category, decimal Price, int Stock);

public static class LinqPart1_FilterProjectOrder
{
    private static readonly List<Product> Products =
    [
        new("Widget",     "Hardware", 9.99m,   120),
        new("Gadget",     "Hardware", 24.99m,  45),
        new("Doohickey",  "Hardware", 4.49m,   300),
        new("Sprocket",   "Parts",    1.99m,   800),
        new("Thingamajig","Parts",    14.99m,  60),
        new("Whatsit",    "Software", 49.99m,  0),
        new("Gizmo",      "Software", 99.99m,  15),
    ];

    public static void Run()
    {
        // ---- Where — filter ----
        var inStock = Products.Where(p => p.Stock > 0);
        var cheap   = Products.Where(p => p.Price < 10m);

        // ---- Select — project / transform ----
        IEnumerable<string> names     = Products.Select(p => p.Name);
        IEnumerable<decimal> prices   = Products.Select(p => p.Price);

        // Anonymous type projection
        var summary = Products.Select(p => new { p.Name, p.Price, IsExpensive = p.Price > 50m });

        // ---- SelectMany — flatten nested sequences ----
        var sentences = new[] { "hello world", "foo bar baz" };
        IEnumerable<string> words = sentences.SelectMany(s => s.Split(' '));
        // ["hello","world","foo","bar","baz"]

        // ---- OrderBy / ThenBy ----
        var byPrice     = Products.OrderBy(p => p.Price);
        var byPriceDesc = Products.OrderByDescending(p => p.Price);
        var multiSort   = Products
            .OrderBy(p => p.Category)
            .ThenByDescending(p => p.Price);

        // ---- Chaining — compose filters ----
        var result = Products
            .Where(p => p.Stock > 0)
            .Where(p => p.Category == "Hardware")
            .OrderBy(p => p.Price)
            .Select(p => $"{p.Name}: ${p.Price}");

        Console.WriteLine("In-stock Hardware (cheapest first):");
        foreach (var line in result)
            Console.WriteLine("  " + line);

        // ---- Take / Skip / TakeLast / SkipLast ----
        var top3       = Products.OrderByDescending(p => p.Price).Take(3);
        var page2      = Products.Skip(2).Take(2);    // simple pagination
        var last2      = Products.TakeLast(2);
        var afterFirst = Products.Skip(1);

        // TakeWhile / SkipWhile — stop/skip based on predicate
        var cheap2 = Products
            .OrderBy(p => p.Price)
            .TakeWhile(p => p.Price < 20m);

        // ---- Distinct ----
        var categories = Products.Select(p => p.Category).Distinct();
        // "Hardware","Parts","Software"

        // ---- DistinctBy (LINQ 6 / .NET 6+) ----
        var onePer = Products.DistinctBy(p => p.Category);

        Console.WriteLine("\nCategories: " + string.Join(", ", categories));
    }
}
