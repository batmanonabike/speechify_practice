// Exercise 14 - Method overloading
// Reference: docs/csharp-refresher/14_Overloading.cs

namespace CSharpExercises;

/// <summary>
/// Overloaded calculation helpers - same name, different signatures.
/// </summary>
public static class MathHelper
{
    /// <summary>Sum of two ints.</summary>
    public static int Add(int a, int b) => throw new NotImplementedException();

    /// <summary>Sum of three ints.</summary>
    public static int Add(int a, int b, int c) => throw new NotImplementedException();

    /// <summary>Sum of a params array of doubles.</summary>
    public static double Add(params double[] values) => throw new NotImplementedException();

    /// <summary>
    /// Clamp <paramref name="value"/> between <paramref name="min"/> and
    /// <paramref name="max"/> (inclusive). Works for any IComparable&lt;T&gt;.
    /// Hint: use generic constraint.
    /// </summary>
    public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        => throw new NotImplementedException();
}
