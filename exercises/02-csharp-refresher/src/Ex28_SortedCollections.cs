// Exercise 28 - SortedDictionary / SortedList
// Reference: docs/csharp-refresher/28_SortedCollections.cs

namespace CSharpExercises;

public static class SortedCollectionExercises
{
    /// <summary>
    /// Given a sequence of (word, score) pairs, return a SortedDictionary
    /// that maps word → score, sorted alphabetically by word.
    /// Where a word appears more than once keep the highest score.
    /// </summary>
    public static SortedDictionary<string, int> BuildLeaderboard(
        IEnumerable<(string Word, int Score)> entries)
        => throw new NotImplementedException();

    /// <summary>
    /// Using a SortedList<int,string>, return the key (int) that ranks at
    /// position <paramref name="rank"/> (0-based) in ascending key order.
    /// Hint: SortedList.Keys[rank].
    /// </summary>
    public static int KeyAtRank(SortedList<int, string> list, int rank)
        => throw new NotImplementedException();

    /// <summary>
    /// Given an unsorted list of integers, return them deduplicated and sorted
    /// descending using a SortedSet with a custom descending comparer.
    /// </summary>
    public static IEnumerable<int> SortedDescendingUnique(IEnumerable<int> source)
        => throw new NotImplementedException();
}
