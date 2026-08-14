// Exercise 14 — Method overloading & properties
// Reference: docs/csharp-refresher/14_Overloading_Properties.cs

namespace CSharpExercises;

/// <summary>
/// A temperature value that can be constructed in Celsius, Fahrenheit, or Kelvin.
/// Internally store the value in Celsius (double).
/// Expose read-only properties: Celsius, Fahrenheit, Kelvin.
/// Hint: F = C * 9/5 + 32   K = C + 273.15
/// </summary>
public class ExerciseTemperature
{
    // TODO: private backing field

    /// <summary>Create from a Celsius value.</summary>
    public static ExerciseTemperature FromCelsius(double celsius)
        => throw new NotImplementedException();

    /// <summary>Create from a Fahrenheit value.</summary>
    public static ExerciseTemperature FromFahrenheit(double fahrenheit)
        => throw new NotImplementedException();

    /// <summary>Create from a Kelvin value.</summary>
    public static ExerciseTemperature FromKelvin(double kelvin)
        => throw new NotImplementedException();

    public double Celsius    => throw new NotImplementedException();
    public double Fahrenheit => throw new NotImplementedException();
    public double Kelvin     => throw new NotImplementedException();
}

/// <summary>
/// Overloaded calculation helpers — same name, different signatures.
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
    /// <paramref name="max"/> (inclusive). Works for any IComparable<T>.
    /// Hint: use generic constraint.
    /// </summary>
    public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        => throw new NotImplementedException();
}
