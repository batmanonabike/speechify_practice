// Exercise 07 - Immutable collections
// Reference: docs/csharp-refresher/07_Immutable.cs

using System.Collections.Immutable;

namespace CSharpExercises;

public static class ImmutableExercises
{
    /// <summary>
    /// Given an ImmutableList<int>, return a new list with all negative values removed.
    /// Do NOT mutate the input — use the immutable API.
    /// Hint: ImmutableList.RemoveAll or rebuild with Where + ToImmutableList.
    /// </summary>
    public static ImmutableList<int> RemoveNegatives(ImmutableList<int> source)
    {
        ImmutableList<int> result = source.RemoveAll(x => x < 0);
        return result;
    }

    /// <summary>
    /// Given an ImmutableDictionary<string,int>, add or update all entries from
    /// <paramref name="updates"/>. Return a new dictionary.
    /// Hint: ImmutableDictionary.SetItems or loop with SetItem.
    /// </summary>
    public static ImmutableDictionary<string, int> ApplyUpdates(
        ImmutableDictionary<string, int> original,
        IEnumerable<KeyValuePair<string, int>> updates)
    {
        return original.SetItems(updates); // Makes a copy.

        //ImmutableDictionary<string, int> result = ImmutableDictionary<string, int>.Empty
        //    .AddRange(original)
        //    .SetItems(updates);
        //return result;
    }

    /// <summary>
    /// Build an ImmutableList<int> containing the squares of 1..n using the Builder
    /// pattern (most efficient for bulk construction).
    /// Hint: ImmutableList.CreateBuilder(), loop, ToImmutable().
    /// </summary>
    public static ImmutableList<int> SquaresUpTo(int n)
    {
        var builder = ImmutableList.CreateBuilder<int>();
        for (int x = 1; x <= n; x++)
            builder.Add(x * x);

        return builder.ToImmutable();
    }
}
