// Exercise 34 - Concurrent collections
// Reference: docs/csharp-refresher/34_ConcurrentCollections.cs

using System.Collections.Concurrent;

namespace CSharpExercises;

public static class ConcurrentExercises
{
    /// <summary>
    /// Increment a counter from 10 parallel tasks, each adding 1000.
    /// Return the final count.
    /// Hint: use ConcurrentDictionary, Interlocked.Increment, or a lock —
    /// do NOT use a plain int without synchronisation.
    /// </summary>
    public static async Task<int> ParallelCounterAsync()
        => throw new NotImplementedException();

    /// <summary>
    /// Producer/consumer: produce integers 0..count-1 via a BlockingCollection<int>
    /// on one task, consume them on another, and return the sum.
    /// Hint: BlockingCollection.CompleteAdding + GetConsumingEnumerable.
    /// </summary>
    public static async Task<long> ProducerConsumerSumAsync(int count)
        => throw new NotImplementedException();

    /// <summary>
    /// Use a ConcurrentDictionary to build a frequency map from
    /// <paramref name="words"/> (multiple threads may call AddOrUpdate
    /// simultaneously if you use Parallel.ForEach).
    /// Return the result as a regular Dictionary.
    /// </summary>
    public static Dictionary<string, int> ConcurrentWordFrequency(IEnumerable<string> words)
        => throw new NotImplementedException();
}
