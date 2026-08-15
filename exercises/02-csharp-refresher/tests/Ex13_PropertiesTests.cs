using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex13_PropertiesTests
{
    [Fact]
    public void Temperature_FromCelsius_RoundTrips()
    {
        var t = ExerciseTemperature.FromCelsius(100);
        Assert.Equal(100,   t.Celsius,    3);
        Assert.Equal(212,   t.Fahrenheit, 3);
        Assert.Equal(373.15, t.Kelvin,    3);
    }

    [Fact]
    public void Temperature_FromFahrenheit_ConvertsCorrectly()
    {
        var t = ExerciseTemperature.FromFahrenheit(32);
        Assert.Equal(0, t.Celsius, 3);
    }

    [Fact]
    public void Temperature_FromKelvin_ConvertsCorrectly()
    {
        var t = ExerciseTemperature.FromKelvin(273.15);
        Assert.Equal(0, t.Celsius, 3);
    }

}
