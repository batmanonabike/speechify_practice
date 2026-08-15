// ============================================================
// IDisposable, using, and resource management
// ============================================================
// IDisposable = deterministic cleanup of unmanaged resources
// (file handles, DB connections, HTTP clients, timers, etc.)
//
// PATTERNS
//   using statement        — auto-calls Dispose at end of block
//   using declaration      — disposes at end of enclosing scope (C# 8+)
//   IDisposable            — synchronous cleanup
//   IAsyncDisposable       — async cleanup (C# 8+)
//   SafeHandle             — advanced: wraps OS handles
// ============================================================

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharpRefresher;

// ============================================================
// 1. Implementing IDisposable correctly
// ============================================================
public class DatabaseConnection : IDisposable
{
    private bool _disposed;

    public DatabaseConnection(string connectionString)
    {
        Console.WriteLine($"[DB] Connection opened: {connectionString}");
    }

    public void Query(string sql)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        Console.WriteLine($"[DB] Executing: {sql}");
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);   // tell GC finaliser is not needed
    }

    // Protected virtual so derived classes can extend cleanup
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            // Free managed resources here
            Console.WriteLine("[DB] Connection closed.");
        }

        // Free unmanaged resources here (if any)
        _disposed = true;
    }

    // Finaliser — safety net if consumer forgets to call Dispose
    ~DatabaseConnection()
    {
        Dispose(disposing: false);
    }
}

// ============================================================
// 2. Sealed class — simplified IDisposable (no inheritance)
// ============================================================
public sealed class TempFile : IDisposable
{
    private bool _disposed;
    public string Path { get; } = System.IO.Path.GetTempFileName();

    public TempFile() => Console.WriteLine($"[TempFile] Created: {Path}");

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        Console.WriteLine($"[TempFile] Deleted: {Path}");
        // System.IO.File.Delete(Path);  — would actually delete in real code
    }
}

// ============================================================
// 3. IAsyncDisposable — for async cleanup (DB, streams, etc.)
// ============================================================
public sealed class AsyncResource : IAsyncDisposable
{
    public AsyncResource() => Console.WriteLine("[AsyncResource] Opened.");

    public async ValueTask DisposeAsync()
    {
        await Task.Delay(1);   // simulate async teardown (flush, close, etc.)
        Console.WriteLine("[AsyncResource] Closed asynchronously.");
    }
}

// ============================================================
// 4. Class implementing BOTH (allows sync or async disposal)
// ============================================================
public sealed class HybridResource : IDisposable, IAsyncDisposable
{
    public HybridResource() => Console.WriteLine("[Hybrid] Opened.");

    public void Dispose()
    {
        Console.WriteLine("[Hybrid] Disposed synchronously.");
    }

    public async ValueTask DisposeAsync()
    {
        await Task.Delay(1);
        Console.WriteLine("[Hybrid] Disposed asynchronously.");
    }
}

// ============================================================
// 5. TTL cache with timer — IDisposable cleans up the timer
// ============================================================
public sealed class ExpiringCache<TKey, TValue> : IDisposable
    where TKey : notnull
{
    private readonly record struct Entry(TValue Value, DateTime ExpiresAt);
    private readonly Dictionary<TKey, Entry> _store = [];
    private readonly System.Threading.Timer _cleanupTimer;
    private readonly TimeSpan _ttl;
    private bool _disposed;

    public ExpiringCache(TimeSpan ttl, TimeSpan? cleanupInterval = null)
    {
        _ttl = ttl;
        _cleanupTimer = new System.Threading.Timer(
            _ => Cleanup(),
            null,
            dueTime: cleanupInterval ?? ttl,
            period: cleanupInterval ?? ttl);
    }

    public void Set(TKey key, TValue value) =>
        _store[key] = new Entry(value, DateTime.UtcNow.Add(_ttl));

    public bool TryGet(TKey key, out TValue? value)
    {
        if (_store.TryGetValue(key, out var entry) && DateTime.UtcNow < entry.ExpiresAt)
        {
            value = entry.Value;
            return true;
        }
        value = default;
        return false;
    }

    private void Cleanup()
    {
        var now = DateTime.UtcNow;
        foreach (var key in _store.Keys.ToList())
            if (_store[key].ExpiresAt <= now)
                _store.Remove(key);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _cleanupTimer.Dispose();
        Console.WriteLine("[ExpiringCache] Timer disposed.");
    }
}

public static class DisposableExamples
{
    public static async Task Run()
    {
        // ============================================================
        // using statement (classic)
        // ============================================================
        Console.WriteLine("=== using statement ===");
        using (var conn = new DatabaseConnection("Server=localhost"))
        {
            conn.Query("SELECT 1");
        }   // Dispose() called here automatically

        // ============================================================
        // using declaration (C# 8+) — disposes at end of scope
        // ============================================================
        Console.WriteLine("\n=== using declaration ===");
        {
            using var temp = new TempFile();
            Console.WriteLine($"Using temp file: {temp.Path}");
        }   // disposed here

        // ============================================================
        // await using — IAsyncDisposable
        // ============================================================
        Console.WriteLine("\n=== await using ===");
        await using (var resource = new AsyncResource())
        {
            Console.WriteLine("[AsyncResource] In use.");
        }   // DisposeAsync() awaited here

        // await using declaration
        {
            await using var hybrid = new HybridResource();
            Console.WriteLine("[Hybrid] In use.");
        }

        // ============================================================
        // Dispose even on exception — using guarantees it
        // ============================================================
        Console.WriteLine("\n=== Dispose on exception ===");
        try
        {
            using var conn2 = new DatabaseConnection("Server=db");
            conn2.Query("SELECT bad");
            throw new InvalidOperationException("Simulated failure");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Caught: {ex.Message}");
            // conn2 was still disposed before the catch block
        }

        // ============================================================
        // TTL cache with disposal
        // ============================================================
        Console.WriteLine("\n=== Expiring cache ===");
        using var cache = new ExpiringCache<string, int>(
            ttl: TimeSpan.FromSeconds(1),
            cleanupInterval: TimeSpan.FromSeconds(2));

        cache.Set("a", 100);
        cache.TryGet("a", out int val);
        Console.WriteLine($"Got: {val}");
    }
}
