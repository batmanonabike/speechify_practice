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
    private readonly double _celsius;

    public double Celsius => _celsius;
    public double Kelvin => CelciusToKelvin(_celsius);
    public double Fahrenheit => CelciusToFahrenheiht(_celsius);

    private ExerciseTemperature(double celsius)
    {
        _celsius = celsius;
    }

    /// <summary>Create from a Celsius value.</summary>
    public static ExerciseTemperature FromCelsius(double celsius) => new(celsius);

    /// <summary>Create from a Fahrenheit value.</summary>
    public static ExerciseTemperature FromFahrenheit(double f) => new(FahrenheitToCelcius(f));
        
    /// <summary>Create from a Kelvin value.</summary>
    public static ExerciseTemperature FromKelvin(double k) => new(KelvinToCelcius(k));

    public static double CelciusToKelvin(double value) => value + 273.15;
    public static double CelciusToFahrenheiht(double value) => value * 9.0 / 5.0 + 32;

    public static double KelvinToCelcius(double value) => value - 273.15;
    public static double FahrenheitToCelcius(double value) => (value - 32) * 5.0 / 9.0;
}

