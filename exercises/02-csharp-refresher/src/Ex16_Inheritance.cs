// Exercise 16 - Inheritance & base class constructors
// Reference: docs/csharp-refresher/16_Inheritance_BaseClass.cs

namespace CSharpExercises;

// ---------------------------------------------------------------
// Your task: implement the class hierarchy below.
// ---------------------------------------------------------------

/// <summary>
/// Abstract base class for all vehicles.
/// Constructor must accept make and year; store them as readonly properties.
/// Abstract method: FuelType() returns a string e.g. "Petrol".
/// Virtual method: Describe() returns "{Make} ({Year}) - {FuelType()}".
/// </summary>
public abstract class Vehicle
{
    // TODO: add Make (string) and Year (int) readonly properties
    // TODO: virtual string Describe() returns "{Make} ({Year}) - {FuelType()}"
    protected Vehicle(string make, int year) { /* TODO: store make and year */ }
    public abstract string FuelType();
    public virtual string Describe() => throw new NotImplementedException();
}

/// <summary>
/// Concrete vehicle. FuelType = "Petrol".
/// Constructor: make, year, engineCC (int).
/// Override Describe() to append " [{engineCC}cc]" to the base description.
/// Hint: call base.Describe() then concatenate.
/// </summary>
public class PetrolCar : Vehicle
{
    // TODO: store engineCC, chain to base constructor
    public PetrolCar(string make, int year, int engineCC)
        : base(make, year) { /* TODO: store engineCC */ }

    public override string FuelType() => throw new NotImplementedException();
    public override string Describe()  => throw new NotImplementedException();
}

/// <summary>
/// Concrete vehicle. FuelType = "Electric".
/// Constructor: make, year, rangeKm (int).
/// Add a property RangeKm.
/// </summary>
public class ElectricCar : Vehicle
{
    // TODO: store rangeKm and call base constructor
    public ElectricCar(string make, int year, int rangeKm)
        : base(make, year) { /* TODO: store rangeKm */ }

    public int RangeKm => throw new NotImplementedException();
    public override string FuelType() => throw new NotImplementedException();
}

/// <summary>
/// Sealed subclass of ElectricCar that adds an AutopilotLevel (int) property.
/// Constructor must chain to ElectricCar via base(make, year, rangeKm).
/// Sealed override of Describe() returns "{base.Describe()} [Autopilot L{level}]".
/// </summary>
public sealed class AutonomousElectricCar : ElectricCar
{
    // TODO: store autopilotLevel, chain to base(make, year, rangeKm)
    public AutonomousElectricCar(string make, int year, int rangeKm, int autopilotLevel)
        : base(make, year, rangeKm) { /* TODO: store autopilotLevel */ }

    public int AutopilotLevel => throw new NotImplementedException();
    public sealed override string Describe() => throw new NotImplementedException();
}
