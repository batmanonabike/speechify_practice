// ============================================================
// LINQ Refresher — Part 2: Aggregation & Grouping
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace CollectionsRefresher;

public static class LinqPart2_AggregateGroup
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
        // ---- Terminal aggregation ----
        int    count    = Products.Count();
        int    inStock  = Products.Count(p => p.Stock > 0);
        decimal total   = Products.Sum(p => p.Price);
        decimal avg     = Products.Average(p => p.Price);
        decimal minP    = Products.Min(p => p.Price);
        decimal maxP    = Products.Max(p => p.Price);

        // MinBy / MaxBy — returns the whole object, not just the value
        Product? cheapest    = Products.MinBy(p => p.Price);
        Product? mostExpensive = Products.MaxBy(p => p.Price);

        Console.WriteLine($"Count: {count}, Total: {total:C}, Avg: {avg:C}");
        Console.WriteLine($"Cheapest: {cheapest?.Name}, Most expensive: {mostExpensive?.Name}");

        // ---- Aggregate (fold / reduce) ----
        // Concatenate names with a separator (illustrative; string.Join is better here)
        string allNames = Products.Aggregate("", (acc, p) =>
            acc == "" ? p.Name : acc + ", " + p.Name);

        // Running product of prices (as int, illustrative)
        decimal priceProduct = Products
            .Select(p => p.Price)
            .Aggregate(1m, (acc, price) => acc * price);

        Console.WriteLine("All names: " + allNames);

        // ---- GroupBy ----
        // Returns IEnumerable<IGrouping<TKey, TElement>>
        IEnumerable<IGrouping<string, Product>> byCategory =
            Products.GroupBy(p => p.Category);

        foreach (IGrouping<string, Product> group in byCategory)
        {
            decimal groupTotal = group.Sum(p => p.Price);
            Console.WriteLine($"\n{group.Key} ({group.Count()} items, ${groupTotal:F2} total):");
            foreach (var p in group.OrderBy(p => p.Price))
                Console.WriteLine($"  {p.Name,-14} ${p.Price,6:F2}  stock: {p.Stock}");
        }

        // GroupBy with result selector — project each group immediately
        var categorySummary = Products
            .GroupBy(
                p => p.Category,
                (key, group) => new
                {
                    Category = key,
                    Count    = group.Count(),
                    AvgPrice = group.Average(p => p.Price),
                    TotalStock = group.Sum(p => p.Stock),
                });

        Console.WriteLine("\nCategory summaries:");
        foreach (var s in categorySummary)
            Console.WriteLine($"  {s.Category}: count={s.Count}, avg=${s.AvgPrice:F2}, stock={s.TotalStock}");

        // ---- ToLookup — like GroupBy but eagerly evaluated into a multimap ----
        // Good when you need to query the same grouping multiple times.
        ILookup<string, Product> lookup = Products.ToLookup(p => p.Category);
        IEnumerable<Product> hardware = lookup["Hardware"];
        IEnumerable<Product> missing  = lookup["NonExistent"];  // empty, not null

        Console.WriteLine($"\nHardware via lookup: {string.Join(", ", hardware.Select(p => p.Name))}");

        // ---- ToDictionary — one-to-one keyed map ----
        // Throws if there are duplicate keys.
        Dictionary<string, Product> byName = Products.ToDictionary(p => p.Name);
        Product widget = byName["Widget"];

        // Safe: project into value type
        Dictionary<string, decimal> priceMap =
            Products.ToDictionary(p => p.Name, p => p.Price);
    }
}
