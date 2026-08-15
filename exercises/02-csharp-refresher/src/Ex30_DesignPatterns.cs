// Exercise 30 - Design Patterns (Strategy, Decorator, Factory)
// Reference: docs/csharp-refresher/30_DesignPatterns.cs

namespace CSharpExercises;

// ---------------------------------------------------------------
// STRATEGY — sorting
// ---------------------------------------------------------------

public interface ISortStrategy<T>
{
    IEnumerable<T> Sort(IEnumerable<T> source);
}

/// <summary>
/// Strategy: sort ascending using the natural order (IComparable<T>).
/// </summary>
public class AscendingSort<T> : ISortStrategy<T> where T : IComparable<T>
{
    public IEnumerable<T> Sort(IEnumerable<T> source)
        => throw new NotImplementedException();
}

/// <summary>
/// Strategy: sort descending using the natural order.
/// </summary>
public class DescendingSort<T> : ISortStrategy<T> where T : IComparable<T>
{
    public IEnumerable<T> Sort(IEnumerable<T> source)
        => throw new NotImplementedException();
}

/// <summary>
/// Context that uses an ISortStrategy.
/// </summary>
public class Sorter<T>
{
    private ISortStrategy<T> _strategy;
    public Sorter(ISortStrategy<T> strategy) => _strategy = strategy;
    public void SetStrategy(ISortStrategy<T> strategy) => _strategy = strategy;
    public IEnumerable<T> Sort(IEnumerable<T> source) => _strategy.Sort(source);
}

// ---------------------------------------------------------------
// DECORATOR — logging around a service
// ---------------------------------------------------------------

public interface IMessageSender
{
    string Send(string message);
}

public class ConsoleSender : IMessageSender
{
    public string Send(string message) => $"SENT: {message}";
}

/// <summary>
/// Decorator: wrap any IMessageSender, prepend a timestamp to the returned string.
/// Format: "[{DateTime.UtcNow:HH:mm:ss}] {inner result}"
/// Hint: store the inner sender in the constructor; delegate Send to it, then decorate.
/// </summary>
public class TimestampedSender : IMessageSender
{
    private readonly IMessageSender _inner;
    public TimestampedSender(IMessageSender inner) => _inner = inner;
    // TODO: delegate to _inner then prepend "[HH:mm:ss] "
    public string Send(string message) => throw new NotImplementedException();
}

// ---------------------------------------------------------------
// FACTORY — shape creation
// ---------------------------------------------------------------

public abstract class ExerciseShape
{
    public abstract double Area();
}

public class ExerciseSquare : ExerciseShape
{
    public double Side { get; }
    public ExerciseSquare(double side) => Side = side;
    public override double Area() => Side * Side;
}

public class ExerciseTriangle : ExerciseShape
{
    public double Base   { get; }
    public double Height { get; }
    public ExerciseTriangle(double @base, double height)
    { Base = @base; Height = height; }
    public override double Area() => 0.5 * Base * Height;
}

/// <summary>
/// Factory method: given a string type ("square" or "triangle") and two
/// doubles (dimension1, dimension2), return the correct ExerciseShape.
/// Throw ArgumentException for unknown types.
/// </summary>
public static class ShapeFactory
{
    public static ExerciseShape Create(string type, double dimension1, double dimension2)
        => throw new NotImplementedException();
}
