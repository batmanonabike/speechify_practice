# Practice Exercises

This repository contains Speechify-style refactoring exercises and a C# refresher study pack.

---

## Exercises

### 01 — Speechify Refactoring & Caching
`exercises/01-speechify-refactoring-caching/`

A realistic refactoring exercise: take legacy code and introduce clean architecture,
dependency injection, and a TTL-based caching layer.

**Open in Visual Studio:** `exercises/01-speechify-refactoring-caching/SpeechifyPractice.sln`

---

### 02 — C# Refresher Exercises
`exercises/02-csharp-refresher/`

Hands-on exercises covering 32 C# topics. Each topic has:
- A **stub file** (`src/`) with guided XML-doc comments and `NotImplementedException` bodies to implement.
- An **xUnit test file** (`tests/`) that goes green once your implementation is correct.

**Workflow:** implement a stub → run its tests → commit → get assessed.

| # | Stub file | Topic |
|---|-----------|-------|
| 01 | `Ex01_Lists.cs` | `List<T>` — filter, sort, deduplicate, rotate |
| 02 | `Ex02_Dictionaries.cs` | Dictionaries — frequency, invert, merge, group |
| 03 | `Ex03_Sets.cs` | HashSet — intersection, symmetric diff, subsets |
| 04 | `Ex04_QueueStackLinkedList.cs` | Queue, Stack, LinkedList |
| 05 | `Ex05_Immutable.cs` | Immutable collections |
| 06 | `Ex06_Linq.cs` | LINQ — filtering & projection |
| 07 | `Ex07_LinqAggregation.cs` | LINQ — grouping & aggregation |
| 08 | `Ex08_LinqJoins.cs` | LINQ — joins & zip |
| 09 | `Ex09_LinqDeferred.cs` | LINQ — deferred execution |
| 10 | `Ex10_SortedCollections.cs` | SortedDictionary, SortedList, SortedSet |
| 11 | `Ex11_Inheritance.cs` | Inheritance & base-class constructors |
| 12 | `Ex12_Interfaces.cs` | Interfaces & multiple implementation |
| 13 | `Ex13_Polymorphism.cs` | Polymorphism & virtual dispatch |
| 14 | `Ex14_OverloadingProperties.cs` | Method overloading & properties |
| 15 | `Ex15_Strings.cs` | String manipulation & StringBuilder |
| 16 | `Ex16_Async.cs` | async/await, Task.WhenAll, IAsyncEnumerable |
| 17 | `Ex17_Generics.cs` | Generics & type constraints |
| 18 | `Ex18_Delegates.cs` | Delegates, Func, Action, events |
| 19 | `Ex19_DesignPatterns.cs` | Strategy, Decorator, Factory patterns |
| 20 | `Ex20_DI.cs` | Dependency injection |
| 21 | `Ex21_RecordsVsClasses.cs` | Records vs classes, `with`-expressions |
| 22 | `Ex22_Nullable.cs` | Nullable reference types & null operators |
| 23 | `Ex23_PatternMatching.cs` | Pattern matching & switch expressions |
| 24 | `Ex24_Extensions.cs` | Extension methods |
| 25 | `Ex25_TestableCode.cs` | Writing testable code & fake dependencies |
| 26 | `Ex26_Disposable.cs` | IDisposable & IAsyncDisposable |
| 27 | `Ex27_Concurrent.cs` | Concurrent collections & thread safety |
| 28 | `Ex28_Arrays.cs` | Arrays, `const`, `Span<T>` |
| 29 | `Ex29_DateTime_TTL.cs` | DateTime, DateTimeOffset & TTL cache |
| 30 | `Ex30_IEquatable.cs` | IEquatable\<T\>, IComparable\<T\>, operators |
| 31 | `Ex31_ValueEquality.cs` | Value equality — class, record, struct, record struct |
| 32 | `Ex32_IComparable.cs` | IComparable\<T\>, IComparer\<T\>, sorting |

---

## Reference Docs
`docs/csharp-refresher/`

Read-only reference examples covering the same 32 topics — useful as an "answer key"
or quick syntax reminder before attempting the exercises.

**Build:** `docs/csharp-refresher/CSharpRefresher.csproj` (console app, `net10.0`)
