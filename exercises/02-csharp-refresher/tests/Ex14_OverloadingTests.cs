using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex14_OverloadingTests
{
    [Fact]
    public void MathHelper_Add_TwoInts()
        => Assert.Equal(5, MathHelper.Add(2, 3));

    [Fact]
    public void MathHelper_Add_ThreeInts()
        => Assert.Equal(6, MathHelper.Add(1, 2, 3));

    [Fact]
    public void MathHelper_Add_ParamsDoubles()
        => Assert.Equal(10.0, MathHelper.Add(1.0, 2.0, 3.0, 4.0));

    [Fact]
    public void MathHelper_Clamp_ClampsToRange()
    {
        Assert.Equal(5, MathHelper.Clamp(3, 5, 10));
        Assert.Equal(10, MathHelper.Clamp(15, 5, 10));
        Assert.Equal(7, MathHelper.Clamp(7, 5, 10));
    }
}
