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
public class ExerciseCircle : IArea, IPerimeter, IScalable
{
    public double Radius { get; }

    public ExerciseCircle(double radius)
        => Radius = radius;

    public double Area()      => throw new NotImplementedException();
    public double Perimeter() => throw new NotImplementedException();
    public IScalable Scale(double factor) => throw new NotImplementedException();
}

/// <summary>
/// Implement IArea, IPerimeter, IScalable.
/// Area = width * height  Perimeter = 2*(width+height)
/// Scale returns a new ExerciseRectangle with both dimensions * factor.
/// </summary>
public class ExerciseRectangle : IArea, IPerimeter, IScalable
{
    public double Width  { get; }
    public double Height { get; }

    public ExerciseRectangle(double width, double height)
    { Width = width; Height = height; }

    public double Area()      => throw new NotImplementedException();
    public double Perimeter() => throw new NotImplementedException();
    public IScalable Scale(double factor) => throw new NotImplementedException();
}

/// <summary>
/// Implement a static helper that sums the areas of any sequence of IArea objects.
/// Hint: LINQ Sum.
/// </summary>
public static class ShapeHelper
{
    public static double TotalArea(IEnumerable<IArea> shapes)
        => throw new NotImplementedException();
}
