// Exercise 20 - async / await
// Reference: docs/csharp-refresher/20_AsyncAwait.cs

namespace CSharpExercises;

public static class AsyncExercises
{
    /// <summary>
    /// Simulate fetching a "user name" by ID with a 10 ms delay (Task.Delay).
    /// Return "User_{id}" if id > 0, otherwise throw ArgumentException.
    /// Must be truly async — no .Result or .Wait().
    /// </summary>
    public static async Task<string> FetchUserNameAsync(int id)
        => throw new NotImplementedException();

    /// <summary>
    /// Call FetchUserNameAsync for EACH id in parallel using Task.WhenAll,
    /// and return the results in the same order as the input ids.
    /// Hint: Select + Task.WhenAll.
    /// </summary>
    public static async Task<string[]> FetchAllAsync(IEnumerable<int> ids)
        => throw new NotImplementedException();

    /// <summary>
    /// Run <paramref name="operation"/> but cancel it after <paramref name="timeoutMs"/> ms.
    /// Return the result on success; throw OperationCanceledException on timeout.
    /// Hint: CancellationTokenSource with CancelAfter; pass the token into the operation.
    /// </summary>
    public static async Task<T> WithTimeoutAsync<T>(
        Func<CancellationToken, Task<T>> operation,
        int timeoutMs)
        => throw new NotImplementedException();

    /// <summary>
    /// Given an IAsyncEnumerable<int> source, return the sum of all values
    /// that satisfy <paramref name="predicate"/>.
    /// Hint: await foreach.
    /// </summary>
    public static async Task<int> SumWhereAsync(
        IAsyncEnumerable<int> source,
        Func<int, bool> predicate)
        => throw new NotImplementedException();
}
