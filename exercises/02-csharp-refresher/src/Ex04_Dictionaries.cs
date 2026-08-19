// Exercise 04 - Dictionary<TKey, TValue>
// Reference: docs/csharp-refresher/04_Dictionaries.cs

using System.Diagnostics;

namespace CSharpExercises;

public static class DictionaryExercises
{
    /// <summary>
    /// Count how many times each word appears in <paramref name="words"/>.
    /// Keys are case-insensitive (normalise to lower-case).
    /// Hint: TryGetValue or GetValueOrDefault to accumulate counts.
    /// </summary>
    public static Dictionary<string, int> WordFrequency(IEnumerable<string> words)
    {
        var dic = new Dictionary<string, int>();
        foreach (var word in words)
        {
            var normalWord = word.ToLower();
            int f = dic.GetValueOrDefault(normalWord, 0) + 1;
            dic[normalWord] = f;
        }
        return dic;
    }

    /// <summary>
    /// Invert the dictionary so values become keys and keys become values.
    /// Assume all values are unique.
    /// </summary>
    public static Dictionary<TValue, TKey> Invert<TKey, TValue>(
        Dictionary<TKey, TValue> source)
        where TKey : notnull
        where TValue : notnull
    {
        var dic = new Dictionary<TValue, TKey>();
        foreach (var kvp in source)
            dic[kvp.Value] = kvp.Key;
        return dic;
    }

    /// <summary>
    /// Merge <paramref name="second"/> into <paramref name="first"/>.
    /// Where both contain the same key, keep the value from <paramref name="second"/>.
    /// Return a new dictionary; do not mutate either input.
    /// </summary>
    public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
        Dictionary<TKey, TValue> first,
        Dictionary<TKey, TValue> second)
        where TKey : notnull
    {
        var dic = new Dictionary<TKey, TValue>(second);
        foreach (var kvp in first)
        {
            if (!second.ContainsKey(kvp.Key))
                dic[kvp.Key] = kvp.Value;
        }
        return dic;
    }

    /// <summary>
    /// Group <paramref name="items"/> by the result of <paramref name="keySelector"/>.
    /// Returns a dictionary where each key maps to the list of matching items.
    /// Hint: TryGetValue + Add, or use GetOrAdd pattern with ??=.
    /// </summary>
    public static Dictionary<TKey, List<T>> GroupBy<T, TKey>(
        IEnumerable<T> items,
        Func<T, TKey> keySelector)
        where TKey : notnull
    {
        var dic = new Dictionary<TKey, List<T>>();
        foreach (var item in items)
        {
            var key = keySelector(item);

            if (!dic.ContainsKey(key))
                dic[key] = [];
            dic[key].Add(item);

            //if (dic.TryGetValue(key, out var values))
            //    values.Add(item);
            //else
            //    dic[key] = [item]; // new List<T>([item]);

        }
        return dic;
    }
}