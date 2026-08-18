// Exercise 24 - Pattern matching
// Reference: docs/csharp-refresher/24_PatternMatching.cs

namespace CSharpExercises;

// Shape hierarchy used throughout this exercise — do not change.
public abstract record Shape2D;
public record Circle2D(double Radius) : Shape2D;
public record Rectangle2D(double W, double H) : Shape2D;
public record Triangle2D(double A, double B, double C) : Shape2D;

public static class PatternMatchingExercises
{
    /// <summary>
    /// Return the area of any Shape2D using a switch expression with
    /// type patterns.  Triangle: Heron's formula.
    /// </summary>
    public static double Area(Shape2D shape)
    {
        ArgumentNullException.ThrowIfNull(shape);

        return shape switch
        {
            Rectangle2D r => r.W * r.H,
            Triangle2D t => Heron(t.A, t.B, t.C),
            Circle2D c => Math.PI * c.Radius * c.Radius,
            _ => throw new ArgumentOutOfRangeException(nameof(shape))
        };

        static double Heron(double a, double b, double c)
        {
            var s = (a + b + c) / 2.0;
            return Math.Sqrt(s * (s - a) * (s - b) * (s - c));
        }
    }

    /// <summary>
    /// Classify an integer as:
    ///   "negative", "zero", "small" (1-9), "large" (>=10)
    /// using a switch expression with relational patterns.
    /// </summary>
    public static string Classify(int n) => n switch
    {
        < 0 => "negative",
        0 => "zero",
        >= 1 and <= 9 => "small",
        _ => "large",
    };

    /// <summary>
    /// Given a nullable string, use a switch expression with
    ///   - null pattern  → return "null"
    ///   - empty string  → return "empty"
    ///   - length < 5    → return "short"
    ///   - otherwise     → return "long"
    /// Hint: property patterns { Length: ... }.
    /// </summary>
    public static string Describe(string? s)
    {
        return s switch
        {
            null => "null",
            { Length: 0 } => "empty",
            { Length: < 5 } => "short",
            _ => "long"
        };
    }

    /// <summary>
    /// Use list patterns (C# 11) to decode simple commands:
    ///   ["MOVE", x, y]  → $"Move to ({x},{y})"
    ///   ["FIRE"]        → "Fire!"
    ///   []              → "Empty"
    ///   _               → "Unknown"
    /// </summary>
    public static string DecodeCommand(string[] parts)
    {
        ArgumentNullException.ThrowIfNull(parts);

        return parts switch
        {

            [] => "Empty",
            ["MOVE", var x, var y] => $"Move to ({x},{y})",
            ["FIRE"] => $"Fire!",
            _ => "Unknown",
        };
    }
}
