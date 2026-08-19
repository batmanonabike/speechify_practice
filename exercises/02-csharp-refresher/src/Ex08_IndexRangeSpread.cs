// Exercise 33 - Index, range, and spread operators
// Reference: docs/csharp-refresher/08_IndexRangeOperators.cs

namespace CSharpExercises;

public static class IndexRangeExercises
{
    /// <summary>
    /// Return the last element using an index-from-end operator.
    /// The source must contain at least one element.
    /// </summary>
    public static T Last<T>(T[] source) => source[^1];

    /// <summary>
    /// Return the final <paramref name="count"/> elements as a new array.
    /// Use a range with an index-from-end bound. The source must contain enough elements.
    /// </summary>
    public static T[] TakeLast<T>(T[] source, int count)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(count, source.Length);
        return source[^count..];
    }

    /// <summary>
    /// Return everything except the first and last elements as a new array.
    /// An array with fewer than two elements returns an empty array.
    /// </summary>
    public static T[] WithoutFirstAndLast<T>(T[] source)
    {
        if (source.Length < 2)
            return [];

        return source[1..^1];
    }

    /// <summary>
    /// Return a new array containing source followed by additions.
    /// Use the spread operator (...) in a collection expression; do not mutate either input.
    /// </summary>
    public static T[] Append<T>(T[] source, params T[] additions) => [.. source, .. additions];
}
