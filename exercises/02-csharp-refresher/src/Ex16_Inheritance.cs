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
public abstract class Vehicle(string make, int year)
{
    public int Year { get; } = year; 
    public string Make { get; } = make;

    public abstract string FuelType();
    public virtual string Describe() => $"{Make} ({Year}) - {FuelType()}";
}

/// <summary>
/// Concrete vehicle. FuelType = "Petrol".
/// Constructor: make, year, engineCC (int).
/// Override Describe() to append " [{engineCC}cc]" to the base description.
/// Hint: call base.Describe() then concatenate.
/// </summary>
// TODO: store engineCC, chain to base constructor
public class PetrolCar(string make, int year, int engineCC) : Vehicle(make, year)
{
    public override string FuelType() => "Petrol";
    public override string Describe()  => $"{base.Describe()} [{engineCC}cc]";
}

/// <summary>
/// Concrete vehicle. FuelType = "Electric".
/// Constructor: make, year, rangeKm (int).
/// Add a property RangeKm.
/// </summary>
public class ElectricCar(string make, int year, int rangeKm) : Vehicle(make, year)
{
    public int RangeKm => rangeKm;
    public override string FuelType() => "Electric";
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
        : base(make, year, rangeKm) { AutopilotLevel = autopilotLevel; }

    public int AutopilotLevel { get; init; }
    public sealed override string Describe() => $"{base.Describe()} [Autopilot L{AutopilotLevel}]";
}
