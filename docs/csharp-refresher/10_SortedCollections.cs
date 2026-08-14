// ============================================================
// SortedDictionary<K,V> and SortedList<K,V>
// ============================================================
// SortedDictionary<K,V>  — Red-Black tree; O(log n) all ops;
//                          efficient insertions/deletions anywhere
// SortedList<K,V>        — parallel arrays; O(log n) lookup via
//                          binary search; O(n) insert/delete;
//                          more cache-friendly & less memory
//
// Both maintain ascending key order during iteration.
// ============================================================

using System;
using System.Collections.Generic;

namespace CollectionsRefresher;

public static class SortedCollectionExamples
{
    public static void Run()
    {
        // ---- SortedDictionary<K,V> ----
        var sd = new SortedDictionary<string, int>
        {
            ["banana"] = 2,
            ["apple"]  = 5,
            ["cherry"] = 1,
        };

        sd["date"] = 3;
        sd.Remove("banana");

        // Iterates in key order: apple, cherry, date
        Console.WriteLine("SortedDictionary (ascending):");
        foreach (var (k, v) in sd)
            Console.WriteLine($"  {k} => {v}");

        // Same lookup API as Dictionary
        if (sd.TryGetValue("apple", out int appleCount))
            Console.WriteLine($"apple: {appleCount}");

        // ---- SortedList<K,V> ----
        var sl = new SortedList<int, string>
        {
            [30] = "thirty",
            [10] = "ten",
            [20] = "twenty",
        };

        // Index-based access (unlike SortedDictionary)
        string firstValue = sl.Values[0];   // "ten" (key order)
        int    firstKey   = sl.Keys[0];     // 10

        int indexOfKey = sl.IndexOfKey(20);     // 1
        int indexOfVal = sl.IndexOfValue("ten"); // 0

        Console.WriteLine($"\nSortedList first: [{firstKey}] = {firstValue}");
        Console.WriteLine($"Index of key 20: {indexOfKey}");

        // ---- When to choose which ----
        // SortedDictionary: many inserts/deletes scattered throughout key space
        // SortedList: read-heavy, compact memory, need index-based access

        // ---- Custom comparer (descending) ----
        var descending = new SortedDictionary<int, string>(Comparer<int>.Create((x, y) => y.CompareTo(x)));
        descending[1] = "one";
        descending[3] = "three";
        descending[2] = "two";

        Console.WriteLine("\nDescending SortedDictionary:");
        foreach (var (k, v) in descending)
            Console.WriteLine($"  {k} => {v}");  // 3,2,1
    }
}
