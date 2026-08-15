// ============================================================
// README - C# Refresher
// ============================================================
//
// FILE MAP - follow this order
// ----------------------------
// FOUNDATIONS AND COLLECTIONS
// 01_ArraysConstAliases.cs       - arrays, const, readonly, aliases
// 02_Strings.cs                  - string creation, indexing, formatting
// 03_Lists.cs                    - List<T>: add, remove, sort, search, slice
// 04_Dictionaries.cs             - Dictionary<TKey,TValue>: CRUD and lookup
// 05_Sets.cs                     - HashSet<T> and SortedSet<T>
// 06_QueueStackLinkedList.cs     - Queue<T>, Stack<T>, LinkedList<T>
// 07_Immutable.cs                - immutable collection variants
// 08_IndexRangeOperators.cs      - index-from-end, ranges, slices, spread
//
// LINQ
// 09_Linq_FilterProjectOrder.cs  - Where, Select, ordering, partitioning
// 10_Linq_AggregateGroup.cs      - aggregation and grouping
// 11_Linq_JoinsZipSets.cs        - joins, Zip, and set operations
// 12_Linq_DeferredAndMisc.cs     - deferred execution and miscellaneous ops
//
// OBJECT-ORIENTED PROGRAMMING
// 13_Properties.cs               - get/set, init, readonly, computed values
// 14_Overloading.cs              - method and operator overloading
// 15_Inheritance_Interfaces.cs   - interfaces and default members
// 16_Inheritance_BaseClass.cs    - inheritance and base constructors
// 17_Polymorphism.cs              - runtime dispatch and pattern matching
// 18_Generics.cs                  - generic types, methods, and constraints
// 19_DelegatesFunc.cs             - delegates, Func, Action, predicates, events
// 20_AsyncAwait.cs                - async/await and asynchronous workflows
//
// TYPE SEMANTICS AND ORDERING
// 21_DateTime_TTL.cs              - date/time types and TTL patterns
// 22_RecordsVsClasses.cs          - records, classes, and structs
// 23_Nullable.cs                  - nullable reference types and null safety
// 24_PatternMatching.cs           - type, property, relational, and list patterns
// 25_ValueEquality_AllCases.cs    - equality across classes, records, and structs
// 26_IEquatable.cs                - typed equality and hash-code contracts
// 27_IComparable.cs               - natural and custom ordering
// 28_SortedCollections.cs         - SortedDictionary<TKey,TValue> and SortedList
//
// ARCHITECTURE AND PRACTICES
// 29_ExtensionMethods.cs          - extension methods
// 30_DesignPatterns.cs             - Decorator, Strategy, Adapter
// 31_DependencyInjection.cs       - dependency injection concepts and patterns
// 32_UnitTestingPatterns.cs       - Arrange/Act/Assert and test doubles
// 33_Disposable.cs                - IDisposable, IAsyncDisposable, using
// 34_ConcurrentCollections.cs     - thread-safe collections and patterns
//
// LINQ CHEAT-SHEET
// Filtering    : Where, OfType, Distinct, DistinctBy
// Projection   : Select, SelectMany
// Ordering     : OrderBy, OrderByDescending, ThenBy, Reverse
// Partitioning : Take, Skip, TakeLast, SkipLast, Chunk
// Grouping     : GroupBy, ToLookup
// Joining      : Join, GroupJoin, Zip
// Set ops      : Union, Intersect, Except, UnionBy, IntersectBy, ExceptBy
// Aggregation  : Count, Sum, Average, Min, Max, MinBy, Aggregate
// Quantifiers  : Any, All, Contains
// Elements     : First, Last, Single, ElementAt
// Conversion   : ToList, ToArray, ToDictionary, ToHashSet
// Generation   : Enumerable.Range, Enumerable.Repeat, Enumerable.Empty
//
// Deferred operators return an IEnumerable<T> and execute later.
// Materializing operators such as ToList, ToArray, Count, and First execute now.
