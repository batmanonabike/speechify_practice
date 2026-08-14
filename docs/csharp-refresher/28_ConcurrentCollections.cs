// ============================================================
// Concurrent collections & thread-safe patterns
// ============================================================
// Use these when multiple threads read/write the same collection.
//
// TYPES
//   ConcurrentDictionary<K,V>  — thread-safe hash map
//   ConcurrentQueue<T>         — thread-safe FIFO
//   ConcurrentStack<T>         — thread-safe LIFO
//   ConcurrentBag<T>           — unordered, thread-safe bag (good for producer/consumer)
//   BlockingCollection<T>      — bounded, blocking producer/consumer
//
// For simple counters / flags: Interlocked class
// For shared state needing complex transactions: lock / SemaphoreSlim
// ============================================================

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpRefresher;

public static class ConcurrentCollectionsExamples
{
    // ============================================================
    // 1. ConcurrentDictionary — the most commonly used
    // ============================================================
    private static void ConcurrentDictionaryDemo()
    {
        var dict = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        // TryAdd — returns false if key exists
        dict.TryAdd("alpha", 1);
        dict.TryAdd("beta", 2);

        // GetOrAdd — atomically adds if missing, returns existing or new
        int val = dict.GetOrAdd("gamma", 3);                      // key  → value factory
        int val2 = dict.GetOrAdd("gamma", key => key.Length);     // factory overload (key not added again)

        // AddOrUpdate — atomically add or transform existing
        dict.AddOrUpdate(
            key: "alpha",
            addValue: 10,
            updateValueFactory: (key, existing) => existing + 1);  // alpha becomes 2

        // TryUpdate — only updates if current value matches
        bool updated = dict.TryUpdate("alpha", newValue: 99, comparisonValue: 2);

        // TryRemove
        bool removed = dict.TryRemove("beta", out int removedVal);

        Console.WriteLine("ConcurrentDictionary:");
        foreach (var (k, v) in dict)
            Console.WriteLine($"  {k} = {v}");
    }

    // ============================================================
    // 2. ConcurrentDictionary as a thread-safe cache
    //    (mirrors what CachedCurrencyRateClient does)
    // ============================================================
    public sealed class ThreadSafeCache<TKey, TValue>(TimeSpan ttl)
        where TKey : notnull
    {
        private readonly record struct Entry(TValue Value, DateTime ExpiresAt);
        private readonly ConcurrentDictionary<TKey, Entry> _store = new();

        public TValue GetOrFetch(TKey key, Func<TKey, TValue> fetch)
        {
            var now = DateTime.UtcNow;

            // Remove stale entry if present
            if (_store.TryGetValue(key, out var existing) && now >= existing.ExpiresAt)
                _store.TryRemove(key, out _);

            // GetOrAdd is NOT atomic for the fetch, but fine for eventually-consistent caches
            return _store.GetOrAdd(key, k =>
            {
                var value = fetch(k);
                return new Entry(value, now.Add(ttl));
            }).Value;
        }

        public bool TryRemove(TKey key) => _store.TryRemove(key, out _);
    }

    // ============================================================
    // 3. ConcurrentQueue — producer/consumer FIFO
    // ============================================================
    private static async Task ConcurrentQueueDemo()
    {
        var queue = new ConcurrentQueue<int>();

        // Producer
        var producer = Task.Run(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                queue.Enqueue(i);
                Thread.Sleep(1);
            }
        });

        // Consumer
        var consumer = Task.Run(async () =>
        {
            int consumed = 0;
            while (consumed < 10)
            {
                if (queue.TryDequeue(out int item))
                {
                    Console.Write(item + " ");
                    consumed++;
                }
                else
                    await Task.Delay(1);
            }
        });

        await Task.WhenAll(producer, consumer);
        Console.WriteLine("\nQueue done.");
    }

    // ============================================================
    // 4. BlockingCollection — bounded buffer with back-pressure
    // ============================================================
    private static async Task BlockingCollectionDemo()
    {
        using var buffer = new BlockingCollection<string>(boundedCapacity: 5);

        var producer = Task.Run(() =>
        {
            foreach (var item in new[] { "a", "b", "c", "d", "e" })
            {
                buffer.Add(item);   // blocks if full
                Console.WriteLine($"[Producer] added {item}");
            }
            buffer.CompleteAdding();   // signal no more items
        });

        var consumer = Task.Run(() =>
        {
            foreach (var item in buffer.GetConsumingEnumerable())
            {
                Thread.Sleep(5);
                Console.WriteLine($"[Consumer] processed {item}");
            }
        });

        await Task.WhenAll(producer, consumer);
    }

    // ============================================================
    // 5. Interlocked — lock-free atomic operations on primitives
    // ============================================================
    private static void InterlockedDemo()
    {
        int counter = 0;

        // Safe increment from multiple threads (no lock needed)
        Parallel.For(0, 1000, _ => Interlocked.Increment(ref counter));
        Console.WriteLine($"Interlocked counter: {counter}");   // always 1000

        // CompareExchange — only sets if current value matches expected
        int original = Interlocked.CompareExchange(ref counter, value: 0, comparand: 1000);
        Console.WriteLine($"Was {original}, now {counter}");   // reset to 0 if it was 1000

        // Interlocked.Add, Exchange, Read (for 64-bit on 32-bit platforms)
        long sum = 0;
        Parallel.For(0L, 100L, i => Interlocked.Add(ref sum, i));
        Console.WriteLine($"Sum 0..99 = {sum}");   // 4950
    }

    // ============================================================
    // 6. SemaphoreSlim — limit concurrency (e.g. max N parallel HTTP calls)
    // ============================================================
    private static async Task SemaphoreDemo()
    {
        using var semaphore = new SemaphoreSlim(initialCount: 2, maxCount: 2);
        var tasks = new List<Task>();

        for (int i = 0; i < 5; i++)
        {
            int id = i;
            tasks.Add(Task.Run(async () =>
            {
                await semaphore.WaitAsync();    // acquire (blocks if 2 already held)
                try
                {
                    Console.WriteLine($"  Task {id} running");
                    await Task.Delay(20);
                }
                finally
                {
                    semaphore.Release();         // always release
                    Console.WriteLine($"  Task {id} released");
                }
            }));
        }

        await Task.WhenAll(tasks);
    }

    public static async Task Run()
    {
        Console.WriteLine("=== ConcurrentDictionary ===");
        ConcurrentDictionaryDemo();

        Console.WriteLine("\n=== Thread-safe cache ===");
        var cache = new ThreadSafeCache<string, decimal>(TimeSpan.FromMinutes(5));
        var rate = cache.GetOrFetch("EUR", k => { Console.WriteLine($"Fetching {k}"); return 1.11m; });
        var cached = cache.GetOrFetch("EUR", k => { Console.WriteLine($"Fetching {k} again"); return 9m; });
        Console.WriteLine($"Rate: {rate}, Cached: {cached}");   // both 1.11 — second call hits cache

        Console.WriteLine("\n=== ConcurrentQueue ===");
        await ConcurrentQueueDemo();

        Console.WriteLine("\n=== BlockingCollection ===");
        await BlockingCollectionDemo();

        Console.WriteLine("\n=== Interlocked ===");
        InterlockedDemo();

        Console.WriteLine("\n=== SemaphoreSlim (max 2 concurrent) ===");
        await SemaphoreDemo();
    }
}
