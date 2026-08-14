using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex12_InterfacesTests
{
    [Fact]
    public void Circle_Area_IsCorrect()
    {
        var c = new ExerciseCircle(5);
        Assert.Equal(Math.PI * 25, c.Area(), 6);
    }

    [Fact]
    public void Circle_Perimeter_IsCorrect()
    {
        var c = new ExerciseCircle(3);
        Assert.Equal(2 * Math.PI * 3, c.Perimeter(), 6);
    }

    [Fact]
    public void Circle_Scale_ReturnsNewCircleWithScaledRadius()
    {
        var c2 = ((ExerciseCircle)new ExerciseCircle(4).Scale(2));
        Assert.Equal(8, c2.Radius);
    }

    [Fact]
    public void Rectangle_Area_IsCorrect()
    {
        var r = new ExerciseRectangle(3, 4);
        Assert.Equal(12, r.Area());
    }

    [Fact]
    public void Rectangle_Perimeter_IsCorrect()
    {
        var r = new ExerciseRectangle(3, 4);
        Assert.Equal(14, r.Perimeter());
    }

    [Fact]
    public void TotalArea_SumsAllShapes()
    {
        var shapes = new IArea[] { new ExerciseCircle(1), new ExerciseRectangle(2,3) };
        var total  = ShapeHelper.TotalArea(shapes);
        Assert.Equal(Math.PI + 6, total, 6);
    }
}
