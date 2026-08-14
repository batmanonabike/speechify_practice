// Exercise 03 — HashSet<T> / SortedSet<T>
// Reference: docs/csharp-refresher/03_Sets.cs

namespace CSharpExercises;

public static class SetExercises
{
    /// <summary>
    /// Return elements that appear in both <paramref name="a"/> and <paramref name="b"/>.
    /// Hint: HashSet.IntersectWith, or LINQ Intersect.
    /// </summary>
    public static IEnumerable<int> Intersection(IEnumerable<int> a, IEnumerable<int> b)
        => throw new NotImplementedException();

    /// <summary>
    /// Return elements that appear in <paramref name="a"/> OR <paramref name="b"/> but not both
    /// (symmetric difference).
    /// Hint: HashSet.SymmetricExceptWith.
    /// </summary>
    public static IEnumerable<int> SymmetricDifference(IEnumerable<int> a, IEnumerable<int> b)
        => throw new NotImplementedException();

    /// <summary>
    /// Return true if <paramref name="subset"/> is a proper subset of <paramref name="superset"/>
    /// (i.e. every element of subset is in superset, AND superset has at least one extra element).
    /// Hint: HashSet.IsProperSubsetOf.
    /// </summary>
    public static bool IsProperSubset(IEnumerable<int> subset, IEnumerable<int> superset)
        => throw new NotImplementedException();

    /// <summary>
    /// Given a list that may contain duplicates, return the duplicate values only
    /// (each duplicate reported once, in ascending order).
    /// e.g. [1,2,2,3,3,3] → [2,3]
    /// Hint: track seen items with a HashSet; collect duplicates in a SortedSet.
    /// </summary>
    public static IEnumerable<int> FindDuplicates(IEnumerable<int> source)
        => throw new NotImplementedException();
}
