// Exercise 33 - Index, range, and spread operators
// Reference: docs/csharp-refresher/08_IndexRangeOperators.cs

namespace CSharpExercises;

public static class IndexRangeExercises
{
    /// <summary>
    /// Return the last element using an index-from-end operator.
    /// The source must contain at least one element.
    /// </summary>
    public static T Last<T>(T[] source)
        => throw new NotImplementedException();

    /// <summary>
    /// Return the final <paramref name="count"/> elements as a new array.
    /// Use a range with an index-from-end bound. The source must contain enough elements.
    /// </summary>
    public static T[] TakeLast<T>(T[] source, int count)
        => throw new NotImplementedException();

    /// <summary>
    /// Return everything except the first and last elements as a new array.
    /// An array with fewer than two elements returns an empty array.
    /// </summary>
    public static T[] WithoutFirstAndLast<T>(T[] source)
        => throw new NotImplementedException();

    /// <summary>
    /// Return a new array containing source followed by additions.
    /// Use the spread operator (...) in a collection expression; do not mutate either input.
    /// </summary>
    public static T[] Append<T>(T[] source, params T[] additions)
        => throw new NotImplementedException();
}
