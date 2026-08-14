// ============================================================
// LINQ Refresher — Part 4: Deferred execution, chunking,
//                          query syntax, and common gotchas
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace CollectionsRefresher;

public static class LinqPart4_DeferredAndMisc
{
    public static void Run()
    {
        // ---- Deferred (lazy) execution ----
        // The query is defined here but NOT executed yet.
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        IEnumerable<int> query = numbers.Where(n => n > 2);

        numbers.Add(6);  // mutate AFTER defining query

        // Execution happens here — includes 6 because it was added before iteration
        Console.WriteLine("Deferred result: " + string.Join(",", query)); // 3,4,5,6

        // Force immediate execution with ToList / ToArray / ToDictionary
        List<int> snapshot = numbers.Where(n => n > 2).ToList();
        numbers.Add(7);
        Console.WriteLine("Snapshot: " + string.Join(",", snapshot)); // 3,4,5,6 — 7 NOT included

        // ---- Chunk — split into batches (.NET 6+) ----
        int[] data = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        IEnumerable<int[]> batches = data.Chunk(3);
        // [[1,2,3],[4,5,6],[7,8,9],[10]]

        Console.WriteLine("Chunks:");
        foreach (int[] batch in batches)
            Console.WriteLine("  [" + string.Join(",", batch) + "]");

        // ---- Range & Repeat (generators) ----
        IEnumerable<int> range  = Enumerable.Range(1, 10);        // 1..10
        IEnumerable<int> evens  = Enumerable.Range(1, 10).Where(n => n % 2 == 0);
        IEnumerable<int> zeros  = Enumerable.Repeat(0, 5);        // 0,0,0,0,0
        IEnumerable<int> empty  = Enumerable.Empty<int>();         // zero elements

        // ---- Index & value together (.NET 9 Index() overload) ----
        // For older targets use Select with index overload:
        var indexed = data.Select((val, idx) => (idx, val));
        foreach (var (idx, val) in indexed.Take(3))
            Console.WriteLine($"  [{idx}] = {val}");

        // ---- Query syntax (SQL-like) vs method syntax ----
        // Both compile to identical IL.

        // Method syntax
        var methodResult = numbers
            .Where(n => n % 2 == 0)
            .Select(n => n * n)
            .OrderByDescending(n => n);

        // Query syntax
        var queryResult =
            from n in numbers
            where n % 2 == 0
            orderby n descending
            select n * n;

        // Group in query syntax
        var grouped =
            from n in data
            group n by n % 3 into g
            orderby g.Key
            select new { Remainder = g.Key, Items = g.ToList() };

        Console.WriteLine("Grouped by mod-3:");
        foreach (var g in grouped)
            Console.WriteLine($"  r={g.Remainder}: {string.Join(",", g.Items)}");

        // Join in query syntax
        var orders    = new[] { (Id: 1, Name: "Alice"), (Id: 2, Name: "Bob") };
        var shipments = new[] { (OrderId: 1, Item: "Widget"), (OrderId: 2, Item: "Gadget"), (OrderId: 1, Item: "Sprocket") };

        var shipped =
            from o in orders
            join s in shipments on o.Id equals s.OrderId
            select new { o.Name, s.Item };

        foreach (var row in shipped)
            Console.WriteLine($"  {row.Name} -> {row.Item}");

        // ---- Common gotchas ----
        // 1. Multiple enumeration: calling Count() + iterating re-runs the query.
        //    Fix: materialise first with ToList().
        IEnumerable<int> lazy = data.Where(n => { Console.Write("?"); return n > 5; });
        // int c = lazy.Count();   // runs filter once
        // foreach (var x in lazy) // runs filter AGAIN

        // 2. Captured variable mutation in closures
        var funcs = new List<Func<int>>();
        for (int i = 0; i < 3; i++)
        {
            int captured = i;                        // capture copy, not the loop var
            funcs.Add(() => captured);
        }
        Console.WriteLine("Closures: " + string.Join(",", funcs.Select(f => f())));  // 0,1,2

        // 3. First() vs FirstOrDefault() — always prefer FirstOrDefault on untrusted data
        int? safeFirst = data.Where(n => n > 100).Cast<int?>().FirstOrDefault();
        Console.WriteLine("Safe first > 100: " + (safeFirst?.ToString() ?? "null"));
    }
}
