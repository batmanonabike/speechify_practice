// Exercise 23 - Nullable reference types & null handling
// Reference: docs/csharp-refresher/23_Nullable.cs

namespace CSharpExercises;

public static class NullableExercises
{
    /// <summary>
    /// Return the length of <paramref name="s"/>, or 0 if it is null.
    /// Hint: null-coalescing operator ??.
    /// </summary>
    public static int SafeLength(string? s)
        => throw new NotImplementedException();

    /// <summary>
    /// Return the first element of <paramref name="list"/> that satisfies
    /// <paramref name="predicate"/>, or null if none found.
    /// The return type must be T? (nullable reference).
    /// Hint: LINQ FirstOrDefault.
    /// </summary>
    public static T? FirstOrNull<T>(IEnumerable<T> list, Func<T, bool> predicate)
        where T : class
        => throw new NotImplementedException();

    /// <summary>
    /// Chain three optional steps together without throwing NullReferenceException.
    /// Given user?.Address?.City, return the city if all parts are non-null,
    /// otherwise return "Unknown".
    /// </summary>
    public static string GetCity(User? user)
        => throw new NotImplementedException();

    /// <summary>
    /// Use the null-coalescing assignment operator (??=) to populate a
    /// cache dictionary: if <paramref name="key"/> is not present, compute
    /// the value using <paramref name="factory"/> and store it.
    /// Return the (possibly newly created) value.
    /// </summary>
    /// NOTE: ??= WILL NOT WORK AGAINST A Dictionary.  This comment is shite.
    public static TValue GetOrAdd<TKey, TValue>(
        Dictionary<TKey, TValue> cache,
        TKey key,
        Func<TKey, TValue> factory)
        where TKey : notnull
        => throw new NotImplementedException();
}

public class User
{
    public string?  Name    { get; init; }
    public Address? Address { get; init; }
}

public class Address
{
    public string? City   { get; init; }
    public string? Street { get; init; }
}
