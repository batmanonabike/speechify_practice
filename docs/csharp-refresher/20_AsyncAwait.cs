// ============================================================
// async / await refresher
// ============================================================
// async/await is syntactic sugar over the Task-based Async Pattern (TAP).
//
// KEY RULES
//   - An async method returns Task, Task<T>, ValueTask, ValueTask<T>, or void
//     (void only for event handlers — fire-and-forget, no error propagation).
//   - await suspends the current method without blocking the thread.
//   - ConfigureAwait(false) avoids capturing the SynchronizationContext
//     (use in library code; not needed in ASP.NET Core or top-level).
//   - Never use .Result or .Wait() — causes deadlocks in sync contexts.
//   - Prefer ValueTask<T> for hot paths that often complete synchronously.
// ============================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpRefresher;

public static class AsyncExamples
{
    // ============================================================
    // 1. BASIC async / await
    // ============================================================

    // Simple async method returning a value
    private static async Task<string> FetchGreetingAsync(string name)
    {
        await Task.Delay(10);    // simulates I/O (e.g. DB or HTTP call)
        return $"Hello, {name}!";
    }

    // Void return — only appropriate for event handlers
    // private static async void OnButtonClick(object sender, EventArgs e) { ... }

    // ============================================================
    // 2. EXCEPTION HANDLING in async code
    // ============================================================
    private static async Task<int> RiskyOperationAsync(bool shouldFail)
    {
        await Task.Delay(5);
        if (shouldFail) throw new InvalidOperationException("Something went wrong.");
        return 42;
    }

    private static async Task ExceptionDemoAsync()
    {
        try
        {
            int result = await RiskyOperationAsync(shouldFail: true);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Caught: {ex.Message}");
        }

        // When awaiting a faulted Task, the first exception is unwrapped from AggregateException.
        // To see ALL exceptions use Task.WhenAll + catch AggregateException.
    }

    // ============================================================
    // 3. CANCELLATION — CancellationToken
    // ============================================================
    private static async Task<string> SlowWorkAsync(CancellationToken ct)
    {
        // Pass ct into every awaitable that accepts it
        await Task.Delay(500, ct);          // throws OperationCanceledException if cancelled
        ct.ThrowIfCancellationRequested();  // manual check between steps
        return "done";
    }

    private static async Task CancellationDemoAsync()
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));

        try
        {
            string result = await SlowWorkAsync(cts.Token);
            Console.WriteLine(result);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Operation was cancelled.");
        }

        // Manual cancellation
        using var cts2 = new CancellationTokenSource();
        cts2.Cancel();   // cancel immediately
        bool isCancelled = cts2.Token.IsCancellationRequested;   // true
    }

    // ============================================================
    // 4. PARALLEL ASYNC — Task.WhenAll & Task.WhenAny
    // ============================================================
    private static async Task<int> DelayedValueAsync(int value, int ms)
    {
        await Task.Delay(ms);
        return value;
    }

    private static async Task ParallelDemoAsync()
    {
        // WhenAll — run all concurrently, await all completions
        // Total time ≈ max(delays) not sum(delays)
        Task<int> t1 = DelayedValueAsync(1, 30);
        Task<int> t2 = DelayedValueAsync(2, 20);
        Task<int> t3 = DelayedValueAsync(3, 10);

        int[] results = await Task.WhenAll(t1, t2, t3);   // [1, 2, 3]
        Console.WriteLine("WhenAll: " + string.Join(", ", results));

        // WhenAny — returns as soon as the FIRST task completes
        Task<int> fast   = DelayedValueAsync(99, 5);
        Task<int> slow   = DelayedValueAsync(0, 200);
        Task<int> winner = await Task.WhenAny(fast, slow);
        Console.WriteLine($"WhenAny winner: {await winner}");

        // WhenAll with exception — ALL tasks run; aggregate errors
        try
        {
            await Task.WhenAll(
                Task.FromException<int>(new ArgumentException("bad")),
                Task.FromException<int>(new InvalidOperationException("also bad")));
        }
        catch (Exception ex)
        {
            // ex is the first exception; to get all, inspect the Task directly
            Console.WriteLine($"WhenAll error: {ex.Message}");
        }
    }

    // ============================================================
    // 5. SEQUENTIAL vs CONCURRENT — a common pitfall
    // ============================================================
    private static async Task SequentialVsConcurrentAsync()
    {
        // SEQUENTIAL — each awaits before the next starts (~60 ms total)
        var a = await DelayedValueAsync(1, 20);
        var b = await DelayedValueAsync(2, 20);
        var c = await DelayedValueAsync(3, 20);

        // CONCURRENT — all start immediately (~20 ms total)
        Task<int> ta = DelayedValueAsync(1, 20);
        Task<int> tb = DelayedValueAsync(2, 20);
        Task<int> tc = DelayedValueAsync(3, 20);
        int ra = await ta;
        int rb = await tb;
        int rc = await tc;

        // Even cleaner with WhenAll:
        var (x, y, z) = await WhenAll3Async(
            DelayedValueAsync(1, 20),
            DelayedValueAsync(2, 20),
            DelayedValueAsync(3, 20));
    }

    private static async Task<(T1, T2, T3)> WhenAll3Async<T1, T2, T3>(
        Task<T1> t1, Task<T2> t2, Task<T3> t3)
    {
        await Task.WhenAll(t1, t2, t3);
        return (t1.Result, t2.Result, t3.Result);  // .Result safe here — tasks are completed
    }

    // ============================================================
    // 6. ValueTask — avoid heap allocation for sync-fast paths
    // ============================================================
    private static readonly Dictionary<string, string> _cache = new();

    private static ValueTask<string> GetFromCacheOrFetchAsync(string key)
    {
        // Hot path: cache hit — complete synchronously, no Task allocation
        if (_cache.TryGetValue(key, out var cached))
            return ValueTask.FromResult(cached);

        // Cold path: must do real async work
        return new ValueTask<string>(FetchAndCacheAsync(key));
    }

    private static async Task<string> FetchAndCacheAsync(string key)
    {
        await Task.Delay(10);   // simulate I/O
        _cache[key] = $"value-for-{key}";
        return _cache[key];
    }

    // ============================================================
    // 7. async IAsyncEnumerable — streaming results (C# 8+)
    // ============================================================
    private static async IAsyncEnumerable<int> GenerateNumbersAsync(
        int count,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
    {
        for (int i = 0; i < count; i++)
        {
            await Task.Delay(5, ct);   // simulate delay per item
            yield return i;
        }
    }

    private static async Task AsyncStreamDemoAsync()
    {
        await foreach (int n in GenerateNumbersAsync(5))
            Console.Write(n + " ");
        Console.WriteLine();

        // With cancellation
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(12));
        try
        {
            await foreach (int n in GenerateNumbersAsync(100, cts.Token))
                Console.Write(n + " ");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("\nStream cancelled.");
        }
    }

    // ============================================================
    // 8. ConfigureAwait
    // ============================================================
    private static async Task ConfigureAwaitDemoAsync()
    {
        // In library code: use ConfigureAwait(false) to avoid
        // capturing/restoring the SynchronizationContext.
        // This prevents deadlocks in WinForms/WPF and improves perf.
        string greeting = await FetchGreetingAsync("World").ConfigureAwait(false);

        // In application (UI / ASP.NET Core) code: omit ConfigureAwait
        // or use ConfigureAwait(true) — the default — to stay on the original context.
        Console.WriteLine(greeting);
    }

    // ============================================================
    // 9. Task.Run — offload CPU-bound work to the thread pool
    // ============================================================
    private static async Task CpuBoundDemoAsync()
    {
        // DON'T do heavy CPU work directly in async method — it blocks the thread.
        // DO wrap it in Task.Run to push it to the thread pool.
        long result = await Task.Run(() =>
        {
            long sum = 0;
            for (long i = 0; i < 1_000_000; i++) sum += i;
            return sum;
        });

        Console.WriteLine($"CPU sum: {result}");
    }

    // ============================================================
    // 10. Async patterns — timeout, retry, fire-and-forget
    // ============================================================
    private static async Task<T> WithTimeoutAsync<T>(Task<T> task, TimeSpan timeout)
    {
        using var cts = new CancellationTokenSource(timeout);
        Task delay = Task.Delay(timeout, cts.Token);

        if (await Task.WhenAny(task, delay) == task)
        {
            cts.Cancel();       // cancel the delay
            return await task;  // propagate exceptions if any
        }

        throw new TimeoutException($"Operation timed out after {timeout.TotalSeconds}s");
    }

    private static async Task<T> RetryAsync<T>(
        Func<Task<T>> operation,
        int maxAttempts = 3,
        TimeSpan? delay = null)
    {
        var backoff = delay ?? TimeSpan.FromMilliseconds(100);
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                return await operation();
            }
            catch when (attempt < maxAttempts)
            {
                Console.WriteLine($"Attempt {attempt} failed, retrying...");
                await Task.Delay(backoff);
                backoff = backoff * 2;   // exponential back-off
            }
        }
        return await operation();   // final attempt — let exception propagate
    }

    // ============================================================
    // ENTRY POINT
    // ============================================================
    public static async Task Run()
    {
        Console.WriteLine("=== 1. Basic async/await ===");
        string greeting = await FetchGreetingAsync("C#");
        Console.WriteLine(greeting);

        Console.WriteLine("\n=== 2. Exception handling ===");
        await ExceptionDemoAsync();

        Console.WriteLine("\n=== 3. Cancellation ===");
        await CancellationDemoAsync();

        Console.WriteLine("\n=== 4. WhenAll / WhenAny ===");
        await ParallelDemoAsync();

        Console.WriteLine("\n=== 5. Sequential vs Concurrent ===");
        await SequentialVsConcurrentAsync();
        Console.WriteLine("(see comments in source — timing not printed here)");

        Console.WriteLine("\n=== 6. ValueTask cache ===");
        _ = await GetFromCacheOrFetchAsync("key1");    // cold
        _ = await GetFromCacheOrFetchAsync("key1");    // warm (sync)
        Console.WriteLine("ValueTask demo complete");

        Console.WriteLine("\n=== 7. IAsyncEnumerable ===");
        await AsyncStreamDemoAsync();

        Console.WriteLine("\n=== 8. ConfigureAwait ===");
        await ConfigureAwaitDemoAsync();

        Console.WriteLine("\n=== 9. CPU-bound Task.Run ===");
        await CpuBoundDemoAsync();

        Console.WriteLine("\n=== 10. Retry pattern ===");
        int attempt = 0;
        int val = await RetryAsync(async () =>
        {
            attempt++;
            if (attempt < 3) throw new Exception("transient");
            await Task.Delay(1);
            return 99;
        });
        Console.WriteLine($"Retry succeeded with value: {val}");
    }
}
