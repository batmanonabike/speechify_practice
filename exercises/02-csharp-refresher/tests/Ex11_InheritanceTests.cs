using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex11_InheritanceTests
{
    [Fact]
    public void PetrolCar_FuelType_IsPetrol()
    {
        var car = new PetrolCar("Ford", 2020, 1600);
        Assert.Equal("Petrol", car.FuelType());
    }

    [Fact]
    public void PetrolCar_Describe_IncludesCC()
    {
        var car = new PetrolCar("Ford", 2020, 1600);
        Assert.Contains("1600cc", car.Describe());
        Assert.Contains("Ford",   car.Describe());
    }

    [Fact]
    public void ElectricCar_FuelType_IsElectric()
    {
        var car = new ElectricCar("Tesla", 2023, 500);
        Assert.Equal("Electric", car.FuelType());
        Assert.Equal(500, car.RangeKm);
    }

    [Fact]
    public void AutonomousElectricCar_Describe_IncludesAutopilotLevel()
    {
        var car = new AutonomousElectricCar("Waymo", 2024, 600, 3);
        Assert.Contains("Autopilot L3", car.Describe());
    }

    [Fact]
    public void Polymorphism_VehicleList_CallsCorrectDescribe()
    {
        var vehicles = new List<Vehicle>
        {
            new PetrolCar("BMW", 2019, 2000),
            new ElectricCar("Tesla", 2022, 400),
        };
        foreach (var v in vehicles)
            Assert.NotEmpty(v.Describe());
    }
}
