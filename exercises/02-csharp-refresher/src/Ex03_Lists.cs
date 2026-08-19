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
        => source.FindAll(x => x > threshold);

    /// <summary>
    /// Return the <paramref name="source"/> list sorted descending (largest first).
    /// Do NOT mutate the original list — return a new sorted copy.
    /// Hint: new List + Sort with a comparer, or LINQ OrderByDescending.
    /// </summary>
    public static List<int> SortDescending(List<int> source)
    {
        var result = new List<int>(source);
        result.Sort((a, b) => b.CompareTo(a));
        return result;
    }

    /// <summary>
    /// Remove all duplicate values and return the unique elements
    /// in the order they first appeared.
    /// Hint: iterate + HashSet to track seen values.
    /// </summary>
    public static List<int> RemoveDuplicates(List<int> source)
    {
        var result = new List<int>();
        var hashSet = new HashSet<int>();

        foreach (var item in source)
        {
            if (hashSet.Add(item))
                result.Add(item);
        }

        return result;
    }

    /// <summary>
    /// Return every element at an even index (0, 2, 4, …).
    /// Hint: loop with index, or LINQ Where with index overload.
    /// </summary>
    public static List<T> ElementsAtEvenIndices<T>(List<T> source)
    {
        var result = new List<T>();
        for (int index = 0; index < source.Count; index += 2)
            result.Add(source[index]);
        return result;
    }

    /// <summary>
    /// Rotate the list left by <paramref name="positions"/>.
    /// e.g. [1,2,3,4,5] rotated 2 → [3,4,5,1,2].
    /// Hint: GetRange + AddRange, or modulo arithmetic.
    /// </summary>
    public static List<T> RotateLeft<T>(List<T> source, int positions)
    {
        var result = source.GetRange(positions, source.Count - positions);
        result.AddRange(source.GetRange(0, positions));
        return result;

        //var lhs = source.GetRange(0, positions);
        //var rhs = source.GetRange(positions, source.Count - positions);

        //var result = new List<T>();
        //result.AddRange(rhs);
        //result.AddRange(lhs);
        //return result;
    }
}
