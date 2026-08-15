// ============================================================
// LINQ Refresher — Part 3: Joins, Zipping, Set ops, Quantifiers
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace CollectionsRefresher;

public record Order(int Id, string CustomerName, string ProductName, int Quantity);
public record Supplier(string ProductName, string SupplierName);

public static class LinqPart3_JoinsZipSets
{
    private static readonly List<Order> Orders =
    [
        new(1, "Alice",  "Widget",  3),
        new(2, "Bob",    "Gadget",  1),
        new(3, "Alice",  "Sprocket",10),
        new(4, "Carol",  "Gizmo",   2),
        new(5, "Bob",    "Widget",  5),
    ];

    private static readonly List<Supplier> Suppliers =
    [
        new("Widget",  "Acme"),
        new("Gadget",  "Globex"),
        new("Gizmo",   "Initech"),
        // Sprocket has no supplier (demonstrates left join)
    ];

    public static void Run()
    {
        // ---- Join (inner join) ----
        var joined = Orders.Join(
            Suppliers,
            order    => order.ProductName,    // outer key
            supplier => supplier.ProductName, // inner key
            (order, supplier) => new          // result selector
            {
                order.CustomerName,
                order.ProductName,
                supplier.SupplierName,
                order.Quantity
            });

        Console.WriteLine("Inner join (orders with known suppliers):");
        foreach (var row in joined)
            Console.WriteLine($"  {row.CustomerName} ordered {row.ProductName} from {row.SupplierName} x{row.Quantity}");

        // ---- GroupJoin (left outer join) ----
        // Every order is kept; supplier list may be empty.
        var leftJoin = Orders.GroupJoin(
            Suppliers,
            o => o.ProductName,
            s => s.ProductName,
            (order, supplierGroup) => new
            {
                order.ProductName,
                Suppliers = supplierGroup.Select(s => s.SupplierName).ToList()
            });

        Console.WriteLine("\nLeft join (all orders, supplier if known):");
        foreach (var row in leftJoin)
        {
            var sup = row.Suppliers.Count > 0 ? string.Join(",", row.Suppliers) : "(none)";
            Console.WriteLine($"  {row.ProductName} — {sup}");
        }

        // Flatten left join to one row per order (classic left-outer pattern)
        var flatLeft = Orders
            .GroupJoin(Suppliers, o => o.ProductName, s => s.ProductName,
                (o, ss) => new { Order = o, Suppliers = ss })
            .SelectMany(
                x => x.Suppliers.DefaultIfEmpty(),
                (x, s) => new { x.Order.CustomerName, x.Order.ProductName, Supplier = s?.SupplierName ?? "Unknown" });

        // ---- Zip — pair two sequences element-by-element ----
        var letters = new[] { "A", "B", "C" };
        var numbers = new[] { 1, 2, 3 };
        var zipped  = letters.Zip(numbers, (l, n) => $"{l}{n}");   // "A1","B2","C3"

        // Three-way zip (.NET 6+)
        var symbols = new[] { "!", "@", "#" };
        var triple  = letters.Zip(numbers).Zip(symbols, (pair, sym) => $"{pair.First}{pair.Second}{sym}");

        Console.WriteLine("\nZipped: " + string.Join(", ", zipped));

        // ---- Set operations on sequences ----
        var a = new[] { 1, 2, 3, 4, 5 };
        var b = new[] { 3, 4, 5, 6, 7 };

        IEnumerable<int> union     = a.Union(b);           // 1,2,3,4,5,6,7 (distinct)
        IEnumerable<int> intersect = a.Intersect(b);       // 3,4,5
        IEnumerable<int> except    = a.Except(b);          // 1,2

        // UnionBy / IntersectBy / ExceptBy (.NET 6+) — compare by key
        var p1 = new[] { new Product("A","X",1m,1), new Product("B","X",2m,2) };
        var p2 = new[] { new Product("B","Y",3m,3), new Product("C","Y",4m,4) };
        IEnumerable<Product> unionByName = p1.UnionBy(p2, p => p.Name);  // A,B,C

        Console.WriteLine("Union: "    + string.Join(",", union));
        Console.WriteLine("Intersect: "+ string.Join(",", intersect));
        Console.WriteLine("Except: "  + string.Join(",", except));

        // ---- Quantifiers ----
        bool anyExpensive = Orders.Any(o => o.Quantity > 5);
        bool allHaveQty   = Orders.All(o => o.Quantity > 0);
        bool noneZero     = !Orders.Any(o => o.Quantity == 0);

        Console.WriteLine($"\nAny qty>5: {anyExpensive}, All qty>0: {allHaveQty}");

        // ---- Element operators ----
        Order  first        = Orders.First();                          // throws if empty
        Order  firstMatch   = Orders.First(o => o.CustomerName == "Bob");
        Order? firstOrNull  = Orders.FirstOrDefault(o => o.Id == 99); // null if not found
        Order  single       = Orders.Single(o => o.Id == 1);          // throws if not exactly 1
        Order? singleOrNull = Orders.SingleOrDefault(o => o.Id == 99);
        Order  last         = Orders.Last();
        Order  elementAt    = Orders.ElementAt(2);                     // 0-based index

        // ---- Concat & Append & Prepend ----
        var more    = new[] { new Order(6, "Dave", "Doohickey", 7) };
        var all     = Orders.Concat(more);
        var withNew = Orders.Append(new Order(7, "Eve", "Thingamajig", 4));
        var withOld = Orders.Prepend(new Order(0, "Zara", "Widget", 1));

        Console.WriteLine($"Total after concat: {all.Count()}");
    }
}
