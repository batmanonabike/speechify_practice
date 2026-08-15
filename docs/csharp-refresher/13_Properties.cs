// ============================================================
// Classes — Properties: get/set, init, readonly, computed
// ============================================================
// Properties expose state with controlled access.
// They look like fields to callers but execute code.
//
// Variants:
//   { get; set; }        — mutable auto-property
//   { get; init; }       — settable only during object initialisation (C# 9+)
//   { get; }             — readonly auto-property (set only in constructor)
//   { get => expr; }     — computed / expression-bodied
//   { get { } set { } }  — full property with backing field
//   required             — caller MUST provide value (C# 11+)
// ============================================================

using System;

namespace CSharpRefresher;

// ---- Auto-properties ----
public class MutablePoint
{
    public double X { get; set; }
    public double Y { get; set; }

    public MutablePoint(double x, double y) { X = x; Y = y; }
    public override string ToString() => $"({X}, {Y})";
}

// ---- Readonly property — set only in constructor ----
public class ImmutablePoint
{
    public double X { get; }
    public double Y { get; }

    public ImmutablePoint(double x, double y) { X = x; Y = y; }

    // Computed property — derived from other state, no backing field needed
    public double DistanceFromOrigin => Math.Sqrt(X * X + Y * Y);

    public override string ToString() => $"({X}, {Y})";
}

// ---- Init-only property (C# 9+) ----
// Allows object initialiser syntax while still being immutable afterwards.
public class Order
{
    public int    Id       { get; init; }
    public string Customer { get; init; } = "";
    public decimal Total   { get; init; }

    // required forces callers to supply the value (C# 11+)
    public required string Reference { get; init; }
}

// ---- Full property with backing field and validation ----
public class Temperature
{
    private double _celsius;

    public double Celsius
    {
        get => _celsius;
        set
        {
            if (value < -273.15)
                throw new ArgumentOutOfRangeException(nameof(value), "Below absolute zero.");
            _celsius = value;
        }
    }

    // Computed from Celsius — no backing field
    public double Fahrenheit
    {
        get => _celsius * 9 / 5 + 32;
        set => Celsius = (value - 32) * 5 / 9;   // setter converts and delegates
    }

    public double Kelvin => _celsius + 273.15;    // expression-bodied, get-only

    public Temperature(double celsius) => Celsius = celsius;  // validation runs here too
}

// ---- Private setter — mutable internally, readonly externally ----
public class Counter
{
    public int Value { get; private set; }

    public void Increment() => Value++;
    public void Reset()     => Value = 0;
}

// ---- Property in interface — contract only, no backing field ----
public interface IHasLabel
{
    string Label { get; }                      // implementor must provide at least a getter
    string LabelUpper => Label.ToUpper();      // default computed property (C# 8+)
}

public class Tag : IHasLabel
{
    public string Label { get; }
    public Tag(string label) => Label = label;
}

// ---- Static property ----
public class AppConfig
{
    private static string _environment = "Production";

    public static string Environment
    {
        get => _environment;
        set => _environment = value ?? throw new ArgumentNullException(nameof(value));
    }

    public static bool IsDevelopment => _environment == "Development";
}

public static class PropertyExamples
{
    public static void Run()
    {
        // ---- Mutable ----
        var mp = new MutablePoint(1, 2);
        mp.X = 10;
        Console.WriteLine(mp);   // (10, 2)

        // ---- Readonly + computed ----
        var ip = new ImmutablePoint(3, 4);
        // ip.X = 5;  — compile error
        Console.WriteLine($"Distance from origin: {ip.DistanceFromOrigin:F4}");  // 5.0000

        // ---- Init-only with object initialiser ----
        var order = new Order { Id = 1, Customer = "Alice", Total = 99.99m, Reference = "ORD-001" };
        // order.Id = 2;  — compile error after construction
        Console.WriteLine($"Order {order.Reference} for {order.Customer}: ${order.Total}");

        // ---- Validated property ----
        var temp = new Temperature(100);
        Console.WriteLine($"{temp.Celsius}°C = {temp.Fahrenheit}°F = {temp.Kelvin}K");

        temp.Fahrenheit = 32;   // sets via Fahrenheit setter → updates _celsius
        Console.WriteLine($"After setting 32°F: {temp.Celsius:F4}°C");

        try { temp.Celsius = -300; }
        catch (ArgumentOutOfRangeException ex) { Console.WriteLine($"Caught: {ex.Message}"); }

        // ---- Private setter ----
        var counter = new Counter();
        counter.Increment();
        counter.Increment();
        // counter.Value = 0;  — compile error
        Console.WriteLine($"Counter: {counter.Value}");

        // ---- Static property ----
        Console.WriteLine($"Env: {AppConfig.Environment}, IsDev: {AppConfig.IsDevelopment}");
        AppConfig.Environment = "Development";
        Console.WriteLine($"Env: {AppConfig.Environment}, IsDev: {AppConfig.IsDevelopment}");

        // ---- Interface property ----
        IHasLabel tag = new Tag("hello");
        Console.WriteLine($"Label: {tag.Label}, Upper: {tag.LabelUpper}");
    }
}
