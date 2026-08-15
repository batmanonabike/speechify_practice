// Exercise 03 - List<T>
// Reference: docs/csharp-refresher/03_Lists.cs

namespace CSharpExercises;

public static class ListExercises
{
    /// <summary>
    /// Return a new list containing only the elements from <paramref name="source"/>
    /// that are greater than <paramref name="threshold"/>, in their original order.
    /// Hint: List<T>.FindAll or LINQ Where + ToList.
    /// </summary>
    public static List<int> FilterAbove(List<int> source, int threshold)
        => throw new NotImplementedException();

    /// <summary>
    /// Return the <paramref name="source"/> list sorted descending (largest first).
    /// Do NOT mutate the original list — return a new sorted copy.
    /// Hint: new List + Sort with a comparer, or LINQ OrderByDescending.
    /// </summary>
    public static List<int> SortDescending(List<int> source)
        => throw new NotImplementedException();

    /// <summary>
    /// Remove all duplicate values and return the unique elements
    /// in the order they first appeared.
    /// Hint: iterate + HashSet to track seen values.
    /// </summary>
    public static List<int> RemoveDuplicates(List<int> source)
        => throw new NotImplementedException();

    /// <summary>
    /// Return every element at an even index (0, 2, 4, …).
    /// Hint: loop with index, or LINQ Where with index overload.
    /// </summary>
    public static List<T> ElementsAtEvenIndices<T>(List<T> source)
        => throw new NotImplementedException();

    /// <summary>
    /// Rotate the list left by <paramref name="positions"/>.
    /// e.g. [1,2,3,4,5] rotated 2 → [3,4,5,1,2].
    /// Hint: GetRange + AddRange, or modulo arithmetic.
    /// </summary>
    public static List<T> RotateLeft<T>(List<T> source, int positions)
        => throw new NotImplementedException();
}
