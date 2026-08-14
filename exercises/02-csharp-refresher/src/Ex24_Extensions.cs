// Exercise 24 — Extension methods
// Reference: docs/csharp-refresher/25_ExtensionMethods.cs

namespace CSharpExercises;

public static class StringExtensions
{
    /// <summary>
    /// Return the string with each word's first letter capitalised.
    /// e.g. "hello world" → "Hello World"
    /// </summary>
    public static string ToTitleCase(this string s)
        => throw new NotImplementedException();

    /// <summary>
    /// Return true if the string is a valid integer (parseable as int).
    /// </summary>
    public static bool IsInteger(this string s)
        => throw new NotImplementedException();
}

public static class EnumerableExtensions
{
    /// <summary>
    /// Return the collection, or an empty sequence if null.
    /// Usage pattern: source.OrEmpty().Where(...)
    /// </summary>
    public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T>? source)
        => throw new NotImplementedException();

    /// <summary>
    /// Partition a sequence into two lists based on a predicate.
    /// Returns (matching, nonMatching).
    /// </summary>
    public static (List<T> Matching, List<T> NonMatching) Partition<T>(
        this IEnumerable<T> source, Func<T, bool> predicate)
        => throw new NotImplementedException();
}

public static class DictionaryExtensions
{
    /// <summary>
    /// Return the value for <paramref name="key"/>, or add-and-return a
    /// default produced by <paramref name="factory"/> if not present.
    /// (Similar to Java's computeIfAbsent.)
    /// </summary>
    public static TValue GetOrAdd<TKey, TValue>(
        this Dictionary<TKey, TValue> dict,
        TKey key,
        Func<TKey, TValue> factory)
        where TKey : notnull
        => throw new NotImplementedException();
}
