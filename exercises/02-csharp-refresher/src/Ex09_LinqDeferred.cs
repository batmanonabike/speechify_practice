// Exercise 09 — LINQ: Deferred execution & lazy evaluation
// Reference: docs/csharp-refresher/09_Linq_Deferred.cs

namespace CSharpExercises;

public static class LinqDeferredExercises
{
    /// <summary>
    /// Split <paramref name="source"/> into batches of <paramref name="size"/>.
    /// Hint: Chunk (NET 6+).
    /// </summary>
    public static IEnumerable<IEnumerable<T>> Batch<T>(IEnumerable<T> source, int size)
        => throw new NotImplementedException();

    /// <summary>
    /// Build a lazy pipeline: filter evens, multiply by 3, take first <paramref name="n"/>.
    /// Return the pipeline as IEnumerable — do NOT materialise it with ToList etc.
    /// Hint: Where + Select + Take (all deferred).
    /// </summary>
    public static IEnumerable<int> LazyEvenTripled(IEnumerable<int> source, int n)
        => throw new NotImplementedException();

    /// <summary>
    /// Demonstrate that LINQ is deferred: given a list, build a query with Where,
    /// then add an item to the list, then materialise — the new item must appear
    /// if it matches the predicate.
    /// Return true if the newly added item appears in the materialised result.
    /// Hint: build query before Add, materialise after Add.
    /// </summary>
    public static bool DeferredExecutionDemo()
        => throw new NotImplementedException();
}
