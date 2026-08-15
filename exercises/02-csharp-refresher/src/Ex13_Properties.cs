// Exercise 13 - Properties
// Reference: docs/csharp-refresher/13_Properties.cs

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

