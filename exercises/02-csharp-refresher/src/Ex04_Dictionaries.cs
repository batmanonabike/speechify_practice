// Exercise 04 - Dictionary<TKey, TValue>
// Reference: docs/csharp-refresher/04_Dictionaries.cs

namespace CSharpExercises;

public static class DictionaryExercises
{
    /// <summary>
    /// Count how many times each word appears in <paramref name="words"/>.
    /// Keys are case-insensitive (normalise to lower-case).
    /// Hint: TryGetValue or GetValueOrDefault to accumulate counts.
    /// </summary>
    public static Dictionary<string, int> WordFrequency(IEnumerable<string> words)
        => throw new NotImplementedException();

    /// <summary>
    /// Invert the dictionary so values become keys and keys become values.
    /// Assume all values are unique.
    /// </summary>
    public static Dictionary<TValue, TKey> Invert<TKey, TValue>(
        Dictionary<TKey, TValue> source)
        where TKey   : notnull
        where TValue : notnull
        => throw new NotImplementedException();

    /// <summary>
    /// Merge <paramref name="second"/> into <paramref name="first"/>.
    /// Where both contain the same key, keep the value from <paramref name="second"/>.
    /// Return a new dictionary; do not mutate either input.
    /// </summary>
    public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
        Dictionary<TKey, TValue> first,
        Dictionary<TKey, TValue> second)
        where TKey : notnull
        => throw new NotImplementedException();

    /// <summary>
    /// Group <paramref name="items"/> by the result of <paramref name="keySelector"/>.
    /// Returns a dictionary where each key maps to the list of matching items.
    /// Hint: TryGetValue + Add, or use GetOrAdd pattern with ??=.
    /// </summary>
    public static Dictionary<TKey, List<T>> GroupBy<T, TKey>(
        IEnumerable<T> items,
        Func<T, TKey> keySelector)
        where TKey : notnull
        => throw new NotImplementedException();
}
