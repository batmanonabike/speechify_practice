// Exercise 15 - Interfaces
// Reference: docs/csharp-refresher/15_Inheritance_Interfaces.cs

namespace CSharpExercises;

// ---------------------------------------------------------------
// Interfaces to implement — do not change the interface definitions.
// ---------------------------------------------------------------

public interface IArea
{
    double Area();
}

public interface IPerimeter
{
    double Perimeter();
}

public interface IScalable
{
    /// Scale all dimensions by <paramref name="factor"/> and return a NEW shape.
    IScalable Scale(double factor);
}

// ---------------------------------------------------------------
// Your task: implement the two concrete shapes below.
// ---------------------------------------------------------------

/// <summary>
/// Implement IArea, IPerimeter, IScalable.
/// Area = π r²  Perimeter = 2πr
/// Scale returns a new ExerciseCircle with radius * factor.
/// </summary>
public class ExerciseCircle(double radius) : IArea, IPerimeter, IScalable
{
    public double Radius { get; } = radius;
    public double Area() => Math.PI * Math.Pow(Radius, 2);
    public double Perimeter() => 2 * Math.PI * Radius;
    public IScalable Scale(double factor) => new ExerciseCircle(Radius * factor);
}

/// <summary>
/// Implement IArea, IPerimeter, IScalable.
/// Area = width * height  Perimeter = 2*(width+height)
/// Scale returns a new ExerciseRectangle with both dimensions * factor.
/// </summary>
public class ExerciseRectangle(double width, double height) : IArea, IPerimeter, IScalable
{
    public double Width { get; } = width;
    public double Height { get; } = height;
    public double Area() => Width * Height;
    public double Perimeter() => Width * 2 + Height * 2;
    public IScalable Scale(double factor) => new ExerciseRectangle(Width * factor, Height * factor);
}

/// <summary>
/// Implement a static helper that sums the areas of any sequence of IArea objects.
/// Hint: LINQ Sum.
/// </summary>
public static class ShapeHelper
{
    public static double TotalArea(IEnumerable<IArea> shapes) => shapes.Sum(a => a.Area());
}
