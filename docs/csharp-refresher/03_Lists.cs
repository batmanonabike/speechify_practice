// ============================================================
// List<T>  —  ordered, index-accessible, resizable
// ============================================================
// Think of it as a dynamic array.  O(1) indexed read/write,
// O(n) insert/remove at arbitrary positions.
// ============================================================

using System;
using System.Collections.Generic;

namespace CollectionsRefresher;

public static class ListExamples
{
    public static void Run()
    {
        // --- Construction ---
        var fruits = new List<string> { "apple", "banana", "cherry" };

        // Add / AddRange
        fruits.Add("date");
        fruits.AddRange(["elderberry", "fig"]);

        // Insert at index
        fruits.Insert(1, "avocado");   // ["apple","avocado","banana",...]

        // Remove by value / by index
        fruits.Remove("banana");
        fruits.RemoveAt(0);

        // --- Access ---
        string first = fruits[0];
        string last  = fruits[^1];   // index-from-end operator

        // --- Search ---
        bool hasFig    = fruits.Contains("fig");
        int  idx       = fruits.IndexOf("cherry");
        int  lastIdx   = fruits.LastIndexOf("cherry");

        // Find (returns default if not found)
        string? found  = fruits.Find(f => f.StartsWith('e'));
        int     foundI = fruits.FindIndex(f => f.Length > 5);

        // --- Sort & reverse ---
        fruits.Sort();                                    // in-place, natural order
        fruits.Sort((a, b) => b.CompareTo(a));           // in-place, descending
        fruits.Reverse();

        // --- Slice ---
        List<string> sub = fruits.GetRange(1, 3);        // index, count

        // --- Capacity hints (avoid re-alloc in hot paths) ---
        var numbers = new List<int>(capacity: 1000);
        for (int i = 0; i < 1000; i++) numbers.Add(i);

        // --- Conversion ---
        string[] arr   = fruits.ToArray();
        var      copy  = new List<string>(fruits);       // shallow copy

        Console.WriteLine($"List count: {fruits.Count}");
        Console.WriteLine($"First: {first}, Last: {last}");
        Console.WriteLine($"Found starting with 'e': {found}");
    }
}
