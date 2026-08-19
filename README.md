# Practice Exercises

This repository contains Speechify-style refactoring exercises and a C# refresher study pack.

---

## Exercises

### 01 — Speechify Refactoring & Caching (warm-up)
`exercises/01-speechify-refactoring-caching/`

A short guided drill on TTL caching and DI wiring. The interfaces and the decorator
shape are pre-written, so this is a warm-up (15–25 minutes) rather than a full rehearsal.
58 tests.

**Open in Visual Studio:** `exercises/01-speechify-refactoring-caching/SpeechifyPractice.sln`

---

### 03 — Billing Refactor & Caching (advanced)
`exercises/03-speechify-refactoring-caching_02/`

The full-fidelity, 50-minute kata. You get a 200-line god class with a static
never-expiring cache, `DateTime.Now` in the business logic, two different rounding modes
and a diverged duplicate of the fee rules — plus a brief. Every seam is your call.

168 tests: 69 characterization tests are green from the start and pin the legacy
behaviour; the other 99 are the work. Ships with `TIMEBOX.md` and `RUBRIC.md`.

**Open in Visual Studio:** `exercises/03-speechify-refactoring-caching_02/SpeechifyKata.slnx`

---

### 02 — C# Refresher Exercises
`exercises/02-csharp-refresher/`

Hands-on exercises covering 34 C# topics in dependency-aware order. Each topic has:
- A **stub file** (`src/`) with guided XML-doc comments and `NotImplementedException` bodies to implement.
- An **xUnit test file** (`tests/`) that goes green once your implementation is correct.

**Workflow:** implement a stub → run its tests → commit → get assessed.

| # | Stub file | Topic |
|---|-----------|-------|
| 01 | `Ex01_ArraysConstAliases.cs` | Arrays, const, readonly, aliases |
| 02 | `Ex02_Strings.cs` | String manipulation and StringBuilder |
| 03 | `Ex03_Lists.cs` | List filtering, sorting, deduplication, rotation |
| 04 | `Ex04_Dictionaries.cs` | Dictionary lookup, merge, and grouping |
| 05 | `Ex05_Sets.cs` | HashSet and set operations |
| 06 | `Ex06_QueueStackLinkedList.cs` | Queue, Stack, LinkedList |
| 07 | `Ex07_Immutable.cs` | Immutable collections |
| 08 | `Ex08_IndexRangeSpread.cs` | Index, range, and spread operators |
| 09 | `Ex09_Linq_Filtering.cs` | LINQ filtering and projection |
| 10 | `Ex10_LinqAggregation.cs` | LINQ grouping and aggregation |
| 11 | `Ex11_LinqJoins.cs` | LINQ joins, Zip, and set operations |
| 12 | `Ex12_LinqDeferred.cs` | LINQ deferred execution |
| 13 | `Ex13_Properties.cs` | Properties and computed values |
| 14 | `Ex14_Overloading.cs` | Method and operator overloading |
| 15 | `Ex15_Interfaces.cs` | Interfaces and multiple implementation |
| 16 | `Ex16_Inheritance.cs` | Inheritance and base constructors |
| 17 | `Ex17_Polymorphism.cs` | Polymorphism and virtual dispatch |
| 18 | `Ex18_Generics.cs` | Generics and type constraints |
| 19 | `Ex19_Delegates.cs` | Delegates, Func, Action, events |
| 20 | `Ex20_Async.cs` | async/await, Task.WhenAll, IAsyncEnumerable |
| 21 | `Ex21_DateTime_TTL.cs` | DateTime, DateTimeOffset, TTL cache |
| 22 | `Ex22_RecordsVsClasses.cs` | Records, classes, and structs |
| 23 | `Ex23_Nullable.cs` | Nullable reference types and null operators |
| 24 | `Ex24_PatternMatching.cs` | Pattern matching and switch expressions |
| 25 | `Ex25_ValueEquality.cs` | Value equality across type kinds |
| 26 | `Ex26_IEquatable.cs` | IEquatable<T> and hash-code contracts |
| 27 | `Ex27_IComparable.cs` | IComparable<T>, IComparer<T>, sorting |
| 28 | `Ex28_SortedCollections.cs` | SortedDictionary, SortedList, SortedSet |
| 29 | `Ex29_Extensions.cs` | Extension methods |
| 30 | `Ex30_DesignPatterns.cs` | Strategy, Decorator, Factory patterns |
| 31 | `Ex31_DI.cs` | Constructor injection and dependency injection |
| 32 | `Ex32_TestableCode.cs` | Testable code and fake dependencies |
| 33 | `Ex33_Disposable.cs` | IDisposable and IAsyncDisposable |
| 34 | `Ex34_Concurrent.cs` | Concurrent collections and thread safety |

---

## Reference Docs
`docs/csharp-refresher/`

Read-only reference examples covering the same 34 topics — useful as an "answer key"
or quick syntax reminder before attempting the exercises.

**Build:** `docs/csharp-refresher/CSharpRefresher.csproj` (console app, `net10.0`)
