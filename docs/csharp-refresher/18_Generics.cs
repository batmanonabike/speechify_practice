// ============================================================
// Generics
// ============================================================
// Generics let you write type-safe, reusable code without
// committing to a concrete type at authoring time.
//
// KEY CONCEPTS
//   - Generic classes, interfaces, methods
//   - Type constraints (where T : ...)
//   - Covariance (out) and contravariance (in) on interfaces
//   - Generic factories and utility patterns
// ============================================================

using System;
using System.Collections.Generic;

namespace CSharpRefresher;

// ============================================================
// 1. Generic class
// ============================================================
public class Box<T>
{
    public T Value { get; }
    public Box(T value) => Value = value;
    public override string ToString() => $"Box<{typeof(T).Name}>({Value})";
}

// ============================================================
// 2. Generic interface
// ============================================================
public interface IGenericRepository<T>
{
    void   Add(T item);
    T?     GetById(int id);
    IEnumerable<T> GetAll();
}

// ============================================================
// 3. Generic class with constraints
// ============================================================
// where T : class        — T must be a reference type
// where T : struct       — T must be a value type
// where T : new()        — T must have a parameterless constructor
// where T : SomeBase     — T must derive from SomeBase
// where T : ISomeInterface — T must implement the interface
// Multiple constraints are combined with commas

public class EntityRepository<T> : IGenericRepository<T> where T : class, new()
{
    private readonly Dictionary<int, T> _store = [];
    private int _nextId = 1;

    public void   Add(T item)          => _store[_nextId++] = item;
    public T?     GetById(int id)      => _store.GetValueOrDefault(id);
    public IEnumerable<T> GetAll()     => _store.Values;
}

// ============================================================
// 4. Generic methods
// ============================================================
public static class GenericUtils
{
    // Swap two values of any type
    public static void Swap<T>(ref T a, ref T b) => (a, b) = (b, a);

    // Return the larger of two IComparable values
    public static T Max<T>(T a, T b) where T : IComparable<T>
        => a.CompareTo(b) >= 0 ? a : b;

    // Safe cast — returns null instead of throwing
    public static TTarget? SafeCast<TSource, TTarget>(TSource source)
        where TTarget : class
        => source as TTarget;

    // Factory using new() constraint
    public static T CreateDefault<T>() where T : new() => new T();

    // Null-safe map (like Optional.map in other languages)
    public static TOut? Map<TIn, TOut>(TIn? value, Func<TIn, TOut> transform)
        where TIn : class
        where TOut : class
        => value is null ? null : transform(value);
}

// ============================================================
// 5. Multiple type parameters
// ============================================================
public class Pair<TFirst, TSecond>(TFirst first, TSecond second)
{
    public TFirst  First  { get; } = first;
    public TSecond Second { get; } = second;

    public Pair<TSecond, TFirst> Swap() => new(Second, First);
    public override string ToString()   => $"({First}, {Second})";
}

// ============================================================
// 6. Covariance (out) and Contravariance (in)
// ============================================================
// Covariant interface: can return a more derived type.
// IEnumerable<Derived> is assignable to IEnumerable<Base> because of 'out'.
public interface IProducer<out T>
{
    T Produce();
}

// Contravariant interface: can accept a more derived type.
// IConsumer<Base> is assignable to IConsumer<Derived> because of 'in'.
public interface IConsumer<in T>
{
    void Consume(T item);
}

public class StringProducer : IProducer<string>
{
    public string Produce() => "hello";
}

public class ObjectConsumer : IConsumer<object>
{
    public void Consume(object item) => Console.WriteLine($"Consumed: {item}");
}

// ============================================================
// 7. Generic with default value
// ============================================================
public class Optional<T>
{
    private readonly T? _value;
    public bool HasValue { get; }

    public Optional(T value)  { _value = value; HasValue = true; }
    private Optional()        { HasValue = false; }

    public static Optional<T> None => new();
    public static Optional<T> Some(T value) => new(value);

    public T Value => HasValue ? _value! : throw new InvalidOperationException("No value.");
    public T GetValueOrDefault(T fallback) => HasValue ? _value! : fallback;
    public Optional<TOut> Map<TOut>(Func<T, TOut> f) =>
        HasValue ? Optional<TOut>.Some(f(_value!)) : Optional<TOut>.None;
}

public static class GenericsExamples
{
    public static void Run()
    {
        // ---- Box ----
        var intBox    = new Box<int>(42);
        var stringBox = new Box<string>("hello");
        Console.WriteLine(intBox);
        Console.WriteLine(stringBox);

        // ---- Swap ----
        int x = 1, y = 2;
        GenericUtils.Swap(ref x, ref y);
        Console.WriteLine($"After swap: x={x}, y={y}");

        // ---- Max ----
        Console.WriteLine(GenericUtils.Max(3, 7));
        Console.WriteLine(GenericUtils.Max("apple", "banana"));

        // ---- Pair ----
        var pair = new Pair<string, int>("age", 30);
        Console.WriteLine(pair);
        Console.WriteLine(pair.Swap());

        // ---- Covariance ----
        IProducer<string> strProducer = new StringProducer();
        IProducer<object> objProducer = strProducer;   // valid because 'out'
        Console.WriteLine(objProducer.Produce());

        // ---- Contravariance ----
        IConsumer<object> objConsumer = new ObjectConsumer();
        IConsumer<string> strConsumer = objConsumer;   // valid because 'in'
        strConsumer.Consume("typed string");

        // ---- Optional ----
        var some = Optional<int>.Some(99);
        var none = Optional<int>.None;
        Console.WriteLine(some.Value);
        Console.WriteLine(none.GetValueOrDefault(-1));
        Console.WriteLine(some.Map(v => v * 2).Value);
    }
}
