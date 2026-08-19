// Exercise 12 - LINQ: Deferred execution & lazy evaluation
// Reference: docs/csharp-refresher/12_Linq_DeferredAndMisc.cs

namespace CSharpExercises;

public static class LinqDeferredExercises
{
    /// <summary>
    /// Split <paramref name="source"/> into batches of <paramref name="size"/>.
    /// Hint: Chunk (NET 6+).
    /// </summary>
    public static IEnumerable<IEnumerable<T>> Batch<T>(IEnumerable<T> source, int size)
    {
        return source.Chunk(size);

        //T[] x = [.. source];
        //IEnumerable<T[]> y = x.Chunk(size);
        //return y;
    }

    /// <summary>
    /// Build a lazy pipeline: filter evens, multiply by 3, take first <paramref name="n"/>.
    /// Return the pipeline as IEnumerable — do NOT materialise it with ToList etc.
    /// Hint: Where + Select + Take (all deferred).
    /// </summary>
    public static IEnumerable<int> LazyEvenTripled(IEnumerable<int> source, int n)
    {
        return source
            .Where(x => x % 2 == 0)
            .Select(y => y * 3)
            .Take(n);
    }

    /// <summary>
    /// Demonstrate that LINQ is deferred: given a list, build a query with Where,
    /// then add an item to the list, then materialise — the new item must appear
    /// if it matches the predicate.
    /// Return true if the newly added item appears in the materialised result.
    /// Hint: build query before Add, materialise after Add.
    /// </summary>
    public static bool DeferredExecutionDemo()
    {
        var list = new List<int>([0, 1, 2, 3, 4, 5]);
        var query = list.Where(x => x == 6);
        list.Add(6);
        return query.FirstOrDefault() == 6;
    }
}
