// ============================================================
// Immutable & read-only collection variants
// ============================================================
// ReadOnlyCollection<T>    — wrapper; O(1) cast, underlying list still mutable
// ImmutableList<T>         — truly immutable; structural sharing
// ImmutableDictionary<K,V> — truly immutable hash map
// ImmutableArray<T>        — immutable value-type wrapper over array
//
// Add System.Collections.Immutable NuGet if not present.
// ============================================================

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace CollectionsRefresher;

public static class ImmutableExamples
{
    public static void Run()
    {
        // ---- ReadOnlyCollection<T> (cheapest "read-only view") ----
        var mutable  = new List<int> { 1, 2, 3 };
        ReadOnlyCollection<int> readOnly = mutable.AsReadOnly();
        // readOnly.Add(4) — compile error; but mutable.Add(4) still affects readOnly

        // ---- ImmutableList<T> ----
        ImmutableList<string> a = ImmutableList.Create("alpha", "beta");
        ImmutableList<string> b = a.Add("gamma");    // new list; a unchanged
        ImmutableList<string> c = b.Remove("alpha");
        ImmutableList<string> d = c.SetItem(0, "BETA");  // replace by index

        Console.WriteLine("a: " + string.Join(",", a));   // alpha,beta
        Console.WriteLine("b: " + string.Join(",", b));   // alpha,beta,gamma

        // Builder pattern for bulk construction (avoids O(n) copies)
        var builder = ImmutableList.CreateBuilder<int>();
        for (int i = 0; i < 5; i++) builder.Add(i * i);
        ImmutableList<int> squares = builder.ToImmutable();

        // ---- ImmutableDictionary<K,V> ----
        ImmutableDictionary<string, int> dict =
            ImmutableDictionary<string, int>.Empty
                .Add("one", 1)
                .Add("two", 2)
                .Add("three", 3);

        ImmutableDictionary<string, int> updated = dict.SetItem("two", 22);  // new dict

        bool found = dict.TryGetValue("one", out int val);

        // ---- ImmutableHashSet<T> ----
        ImmutableHashSet<int> set1 = ImmutableHashSet.Create(1, 2, 3);
        ImmutableHashSet<int> set2 = set1.Add(4).Remove(1);
        ImmutableHashSet<int> union = set1.Union(set2);

        // ---- ImmutableArray<T> (value type — no heap alloc for the wrapper) ----
        ImmutableArray<double> arr = ImmutableArray.Create(1.1, 2.2, 3.3);
        ImmutableArray<double> arr2 = arr.Add(4.4);

        Console.WriteLine("Squares: " + string.Join(",", squares));
    }
}
