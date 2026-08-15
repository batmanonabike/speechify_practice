// Exercise 18 - Generics
// Reference: docs/csharp-refresher/18_Generics.cs

namespace CSharpExercises;

/// <summary>
/// A simple fixed-capacity generic stack backed by an array.
/// Push throws InvalidOperationException when full.
/// Pop/Peek throw InvalidOperationException when empty.
/// </summary>
public class BoundedStack<T>
{
    private readonly T[] _data;
    private int _top = -1;

    public BoundedStack(int capacity)
        => _data = new T[capacity];

    public int  Count    => _top + 1;
    public bool IsEmpty  => _top < 0;
    public bool IsFull   => _top == _data.Length - 1;

    public void Push(T item)  => throw new NotImplementedException();
    public T    Pop()         => throw new NotImplementedException();
    public T    Peek()        => throw new NotImplementedException();
}

/// <summary>
/// Generic utility methods demonstrating constraints.
/// </summary>
public static class GenericUtils
{
    /// <summary>
    /// Return the larger of two IComparable<T> values.
    /// </summary>
    public static T Max<T>(T a, T b) where T : IComparable<T>
        => throw new NotImplementedException();

    /// <summary>
    /// Convert any nullable value to its non-null equivalent,
    /// or return <paramref name="fallback"/> if null.
    /// </summary>
    public static T Coalesce<T>(T? value, T fallback) where T : class
        => throw new NotImplementedException();

    /// <summary>
    /// Given a sequence, return distinct elements using a key extracted by
    /// <paramref name="keySelector"/> — keeping the first occurrence.
    /// Hint: HashSet of keys + yield return.
    /// </summary>
    public static IEnumerable<T> DistinctBy<T, TKey>(
        IEnumerable<T> source,
        Func<T, TKey> keySelector)
        => throw new NotImplementedException();
}
