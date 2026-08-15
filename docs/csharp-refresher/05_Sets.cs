// ============================================================
// HashSet<T>  —  unordered set of unique values
// SortedSet<T> — sorted set (Red-Black tree, O(log n))
// ============================================================
// Use HashSet when you care about membership, not order.
// Use SortedSet when you need membership AND sorted traversal.
// ============================================================

using System;
using System.Collections.Generic;

namespace CollectionsRefresher;

public static class SetExamples
{
    public static void Run()
    {
        // ---- HashSet<T> ----
        var a = new HashSet<int> { 1, 2, 3, 4, 5 };
        var b = new HashSet<int> { 3, 4, 5, 6, 7 };

        // Add returns false if item already present
        bool added = a.Add(6);    // true
        bool dupe  = a.Add(1);    // false — already there

        bool has3 = a.Contains(3);  // O(1)
        a.Remove(1);

        // --- Set operations (mutate the receiver) ---
        var union        = new HashSet<int>(a); union.UnionWith(b);         // a | b
        var intersection = new HashSet<int>(a); intersection.IntersectWith(b); // a & b
        var difference   = new HashSet<int>(a); difference.ExceptWith(b);   // a - b
        var symmetric    = new HashSet<int>(a); symmetric.SymmetricExceptWith(b); // a ^ b

        // --- Subset / superset ---
        var small = new HashSet<int> { 3, 4 };
        bool isSubset   = small.IsSubsetOf(a);
        bool isSuperset = a.IsSupersetOf(small);
        bool overlaps   = a.Overlaps(b);
        bool setEqual   = a.SetEquals(b);

        Console.WriteLine($"Union count: {union.Count}");
        Console.WriteLine($"Intersection: {string.Join(",", intersection)}");

        // ---- SortedSet<T> ----
        var sorted = new SortedSet<int> { 5, 3, 1, 4, 2 };
        // Iterates in ascending order: 1,2,3,4,5
        Console.WriteLine("SortedSet: " + string.Join(",", sorted));

        int min = sorted.Min;   // 1
        int max = sorted.Max;   // 5

        // Range view — items in [2, 4]
        SortedSet<int> view = sorted.GetViewBetween(2, 4);  // {2,3,4}

        // Reverse
        IEnumerable<int> desc = sorted.Reverse();

        // ---- String set, case-insensitive ----
        var tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "CSharp", "dotnet", "LINQ"
        };
        bool hasDotnet = tags.Contains("DotNet");  // true
    }
}
