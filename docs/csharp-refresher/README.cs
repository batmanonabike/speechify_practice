// ============================================================
// README — C# Refresher
// ============================================================
//
// FILE MAP
// --------
// COLLECTIONS
// 01_Lists.cs                 — List<T>: add, remove, sort, search, slice
// 02_Dictionaries.cs          — Dictionary<K,V>: CRUD, safe lookup, merge
// 03_Sets.cs                  — HashSet<T> & SortedSet<T>: set algebra
// 04_QueueStackLinkedList.cs  — Queue<T>, Stack<T>, LinkedList<T>
// 05_Immutable.cs             — ImmutableList/Dictionary/HashSet/Array
// 10_SortedCollections.cs     — SortedDictionary<K,V> & SortedList<K,V>
//
// LINQ
// 06_Linq_FilterProjectOrder.cs — Where, Select, SelectMany, OrderBy, Take/Skip
// 07_Linq_AggregateGroup.cs     — Count/Sum/Avg/Min/Max, GroupBy, ToLookup
// 08_Linq_JoinsZipSets.cs       — Join, GroupJoin, Zip, Union/Intersect/Except
// 09_Linq_DeferredAndMisc.cs    — Deferred exec, Chunk, Range, query syntax, gotchas
//
// CLASSES & OOP
// 11_Inheritance_BaseClass.cs — abstract/virtual/override/sealed, base() constructor
// 12_Inheritance_Interfaces.cs— interface, multiple impl, explicit impl, default members
// 13_Polymorphism.cs          — runtime dispatch, new (hiding), pattern matching, is/as
// 14_Overloading.cs           — method overloading, params, operator overloading
// 15_Properties.cs            — get/set, init-only, readonly, computed, validated, static
//
//
// QUICK CHEAT-SHEET
// -----------------
//
// WHEN TO USE WHAT
// ┌─────────────────────────┬────────────────────────────────────────────┐
// │ Need                    │ Use                                        │
// ├─────────────────────────┼────────────────────────────────────────────┤
// │ Ordered, index access   │ List<T>  /  T[]  /  ImmutableArray<T>     │
// │ Key→Value lookup        │ Dictionary<K,V>                            │
// │ Key→Value, sorted iter  │ SortedDictionary<K,V>                      │
// │ Key→Value, index access │ SortedList<K,V>                            │
// │ Unique membership test  │ HashSet<T>                                 │
// │ Unique + sorted         │ SortedSet<T>                               │
// │ FIFO                    │ Queue<T>                                   │
// │ LIFO                    │ Stack<T>                                   │
// │ O(1) mid-list insert    │ LinkedList<T>                              │
// │ Thread-safe dict        │ ConcurrentDictionary<K,V>                  │
// │ Immutable sharing       │ ImmutableList/Dictionary/HashSet           │
// └─────────────────────────┴────────────────────────────────────────────┘
//
// KEY LINQ OPERATORS
// ──────────────────
// Filtering    : Where, OfType, Distinct, DistinctBy
// Projection   : Select, SelectMany
// Ordering     : OrderBy, OrderByDescending, ThenBy, ThenByDescending, Reverse
// Partitioning : Take, Skip, TakeLast, SkipLast, TakeWhile, SkipWhile, Chunk
// Grouping     : GroupBy, ToLookup
// Joining      : Join, GroupJoin, Zip
// Set ops      : Union, Intersect, Except, UnionBy, IntersectBy, ExceptBy
// Aggregation  : Count, Sum, Average, Min, Max, MinBy, MaxBy, Aggregate
// Quantifiers  : Any, All, Contains
// Elements     : First[OrDefault], Last[OrDefault], Single[OrDefault], ElementAt
// Conversion   : ToList, ToArray, ToDictionary, ToHashSet, AsEnumerable
// Generation   : Enumerable.Range, Enumerable.Repeat, Enumerable.Empty
// Misc         : Append, Prepend, Concat, SequenceEqual
//
// DEFERRED vs IMMEDIATE
// ──────────────────────
// Deferred  (lazy)  : Where, Select, OrderBy, GroupBy, Join, …
// Immediate (eager) : ToList, ToArray, ToDictionary, Count, Sum, First, Any, …
//
// Rule of thumb: if the return type is IEnumerable<T> → deferred.
//                If the return type is a concrete collection or scalar → immediate.
