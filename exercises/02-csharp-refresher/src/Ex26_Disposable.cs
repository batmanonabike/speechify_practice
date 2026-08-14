// Exercise 26 — IDisposable & using
// Reference: docs/csharp-refresher/27_Disposable.cs

namespace CSharpExercises;

/// <summary>
/// A resource that tracks whether it has been disposed.
/// Implement the full IDisposable pattern (including a finalizer guard).
/// Rules:
///   • IsDisposed must return true after Dispose() is called.
///   • Read() must throw ObjectDisposedException if called after disposal.
///   • Repeated calls to Dispose() must be safe (idempotent).
/// Hint: use a bool _disposed field; suppress the finalizer in Dispose().
/// </summary>
public class ManagedResource : IDisposable
{
    private bool _disposed;

    public bool IsDisposed => _disposed;

    public string Read()
        => throw new NotImplementedException();

    public void Dispose()
        => throw new NotImplementedException();
}

/// <summary>
/// Async resource — implement IAsyncDisposable.
/// DisposeAsync should set _disposed and complete a Task.
/// </summary>
public class AsyncManagedResource : IAsyncDisposable
{
    private bool _disposed;
    public bool IsDisposed => _disposed;

    public ValueTask DisposeAsync()
        => throw new NotImplementedException();
}
