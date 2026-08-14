// Exercise 23 — Pattern matching
// Reference: docs/csharp-refresher/24_PatternMatching.cs

namespace CSharpExercises;

// Shape hierarchy used throughout this exercise — do not change.
public abstract record Shape2D;
public record Circle2D(double Radius)        : Shape2D;
public record Rectangle2D(double W, double H): Shape2D;
public record Triangle2D(double A, double B, double C) : Shape2D;

public static class PatternMatchingExercises
{
    /// <summary>
    /// Return the area of any Shape2D using a switch expression with
    /// type patterns.  Triangle: Heron's formula.
    /// </summary>
    public static double Area(Shape2D shape)
        => throw new NotImplementedException();

    /// <summary>
    /// Classify an integer as:
    ///   "negative", "zero", "small" (1-9), "large" (>=10)
    /// using a switch expression with relational patterns.
    /// </summary>
    public static string Classify(int n)
        => throw new NotImplementedException();

    /// <summary>
    /// Given a nullable string, use a switch expression with
    ///   - null pattern  → return "null"
    ///   - empty string  → return "empty"
    ///   - length < 5    → return "short"
    ///   - otherwise     → return "long"
    /// Hint: property patterns { Length: ... }.
    /// </summary>
    public static string Describe(string? s)
        => throw new NotImplementedException();

    /// <summary>
    /// Use list patterns (C# 11) to decode simple commands:
    ///   ["MOVE", x, y]  → $"Move to ({x},{y})"
    ///   ["FIRE"]        → "Fire!"
    ///   []              → "Empty"
    ///   _               → "Unknown"
    /// </summary>
    public static string DecodeCommand(string[] parts)
        => throw new NotImplementedException();
}
