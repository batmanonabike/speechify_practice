// Exercise 05 — Immutable collections
// Reference: docs/csharp-refresher/05_Immutable.cs

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
        => throw new NotImplementedException();

    /// <summary>
    /// Given an ImmutableDictionary<string,int>, add or update all entries from
    /// <paramref name="updates"/>. Return a new dictionary.
    /// Hint: ImmutableDictionary.SetItems or loop with SetItem.
    /// </summary>
    public static ImmutableDictionary<string, int> ApplyUpdates(
        ImmutableDictionary<string, int> original,
        IEnumerable<KeyValuePair<string, int>> updates)
        => throw new NotImplementedException();

    /// <summary>
    /// Build an ImmutableList<int> containing the squares of 1..n using the Builder
    /// pattern (most efficient for bulk construction).
    /// Hint: ImmutableList.CreateBuilder(), loop, ToImmutable().
    /// </summary>
    public static ImmutableList<int> SquaresUpTo(int n)
        => throw new NotImplementedException();
}
