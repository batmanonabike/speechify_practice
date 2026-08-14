// ============================================================
// Pattern Matching (deeper)
// ============================================================
// Pattern matching lets you test a value's shape and
// extract parts of it in a single expression.
//
// PATTERNS COVERED
//   Type        — is T  /  case T t
//   Declaration — is T name
//   Constant    — is 42  /  is null  /  is "foo"
//   Relational  — is > 0   is <= 100
//   Logical     — and / or / not
//   Property    — is { Prop: value }
//   Positional  — is (x, y)   (uses Deconstruct)
//   List        — [first, .., last]  (C# 11)
//   var         — is var x  (always matches, binds value)
//   Discard     — _  (wildcard)
//   Guard       — when clause
// ============================================================

using System;
using System.Collections.Generic;

namespace CSharpRefresher;

public static class PatternMatchingExamples
{
    // ============================================================
    // 1. is expression — type + declaration patterns
    // ============================================================
    private static string Describe(object? obj)
    {
        if (obj is null)             return "null";
        if (obj is int i)            return $"int: {i}";
        if (obj is string { Length: > 5 } s) return $"long string: {s}";  // property guard
        if (obj is string s2)        return $"short string: {s2}";
        return $"other: {obj.GetType().Name}";
    }

    // ============================================================
    // 2. switch expression — exhaustive, returns a value
    // ============================================================
    private enum TrafficLight { Red, Amber, Green }

    private static string GetInstruction(TrafficLight light) => light switch
    {
        TrafficLight.Red   => "Stop",
        TrafficLight.Amber => "Prepare",
        TrafficLight.Green => "Go",
        _                  => throw new ArgumentOutOfRangeException(nameof(light))
    };

    // ============================================================
    // 3. Relational and logical patterns
    // ============================================================
    private static string ClassifyScore(int score) => score switch
    {
        < 0 or > 100 => "Invalid",
        >= 90        => "A",
        >= 80        => "B",
        >= 70        => "C",
        >= 60        => "D",
        _            => "F",
    };

    private static string ClassifyTemperature(double celsius) => celsius switch
    {
        < -40                => "Extreme cold",
        >= -40 and < 0       => "Freezing",
        >= 0   and < 15      => "Cold",
        >= 15  and < 25      => "Comfortable",
        >= 25  and < 35      => "Warm",
        _                    => "Hot",
    };

    // ============================================================
    // 4. Property patterns
    // ============================================================
    private record OrderRecord2(decimal Amount, string Status, string Country);

    private static decimal GetDiscount(OrderRecord2 order) => order switch
    {
        { Status: "VIP",    Amount: > 100 }       => 0.20m,
        { Status: "VIP" }                         => 0.10m,
        { Country: "US",    Amount: > 500 }       => 0.05m,
        { Amount: > 1000 }                        => 0.03m,
        _                                         => 0m,
    };

    // Nested property pattern
    private record ShippingAddress(string Country, string? State);
    private record Purchase(decimal Total, ShippingAddress Address);

    private static decimal GetTaxRate(Purchase purchase) => purchase switch
    {
        { Address.Country: "US", Address.State: "CA" } => 0.0725m,
        { Address.Country: "US", Address.State: "TX" } => 0.0625m,
        { Address.Country: "US" }                      => 0.05m,
        { Address.Country: "GB" }                      => 0.20m,
        _                                              => 0m,
    };

    // ============================================================
    // 5. Positional patterns (uses Deconstruct)
    // ============================================================
    private record Vector(double X, double Y)
    {
        public void Deconstruct(out double x, out double y) => (x, y) = (X, Y);
    }

    private static string ClassifyVector(Vector v) => v switch
    {
        (0, 0)             => "Origin",
        (var x, 0) when x > 0 => "Positive X-axis",
        (0, var y) when y > 0 => "Positive Y-axis",
        (var x, var y) when x > 0 && y > 0 => "Quadrant I",
        _                  => "Other",
    };

    // ============================================================
    // 6. List patterns (C# 11)
    // ============================================================
    private static string DescribeList(int[] nums) => nums switch
    {
        []                => "empty",
        [var single]      => $"one element: {single}",
        [var first, var second] => $"two: {first}, {second}",
        [var first, .., var last] => $"many, first={first} last={last}",
    };

    // ============================================================
    // 7. Pattern matching in if / foreach
    // ============================================================
    private static void ProcessItems(IEnumerable<object?> items)
    {
        foreach (var item in items)
        {
            if (item is int n and > 0)
                Console.WriteLine($"  positive int: {n}");
            else if (item is string { Length: > 0 } s)
                Console.WriteLine($"  non-empty string: {s}");
            else if (item is null)
                Console.WriteLine("  null");
            else
                Console.WriteLine($"  other: {item}");
        }
    }

    // ============================================================
    // 8. Replacing polymorphism with switch expression
    //    (useful when you can't or don't want to add virtual methods)
    // ============================================================
    private abstract record Shape2;
    private record Circle2(double Radius) : Shape2;
    private record Rectangle2(double Width, double Height) : Shape2;
    private record Triangle2(double Base, double Height) : Shape2;

    private static double Area(Shape2 shape) => shape switch
    {
        Circle2    c  => Math.PI * c.Radius * c.Radius,
        Rectangle2 r  => r.Width * r.Height,
        Triangle2  t  => 0.5 * t.Base * t.Height,
        _             => throw new NotSupportedException(shape.GetType().Name),
    };

    public static void Run()
    {
        Console.WriteLine("=== is / declaration ===");
        foreach (object? o in new object?[] { null, 42, "hi", "toolong", 3.14 })
            Console.WriteLine($"  {Describe(o)}");

        Console.WriteLine("\n=== switch expression — enum ===");
        foreach (var light in Enum.GetValues<TrafficLight>())
            Console.WriteLine($"  {light} => {GetInstruction(light)}");

        Console.WriteLine("\n=== relational patterns ===");
        foreach (int s in new[] { -1, 55, 65, 75, 85, 95 })
            Console.WriteLine($"  {s} => {ClassifyScore(s)}");

        foreach (double t in new[] { -50.0, -10, 5, 20, 30, 40 })
            Console.WriteLine($"  {t}°C => {ClassifyTemperature(t)}");

        Console.WriteLine("\n=== property patterns ===");
        var orders = new[]
        {
            new OrderRecord2(150m, "VIP",      "US"),
            new OrderRecord2(50m,  "VIP",      "UK"),
            new OrderRecord2(600m, "Standard", "US"),
            new OrderRecord2(200m, "Standard", "DE"),
        };
        foreach (var o in orders)
            Console.WriteLine($"  {o} => {GetDiscount(o):P0} discount");

        Console.WriteLine("\n=== nested property patterns ===");
        var purchases = new[]
        {
            new Purchase(100m, new ShippingAddress("US", "CA")),
            new Purchase(100m, new ShippingAddress("US", "TX")),
            new Purchase(100m, new ShippingAddress("GB", null)),
        };
        foreach (var p in purchases)
            Console.WriteLine($"  {p.Address.Country}/{p.Address.State} => {GetTaxRate(p):P2} tax");

        Console.WriteLine("\n=== positional patterns ===");
        var vectors = new[] { new Vector(0,0), new Vector(1,0), new Vector(0,1), new Vector(2,3), new Vector(-1,2) };
        foreach (var v in vectors)
            Console.WriteLine($"  {v} => {ClassifyVector(v)}");

        Console.WriteLine("\n=== list patterns ===");
        foreach (var arr in new[] { new int[]{}, new[]{1}, new[]{1,2}, new[]{1,2,3,4,5} })
            Console.WriteLine($"  [{string.Join(",",arr)}] => {DescribeList(arr)}");

        Console.WriteLine("\n=== mixed items ===");
        ProcessItems(new object?[] { 5, -2, "hello", "", null, 3.14 });

        Console.WriteLine("\n=== shape area (switch replaces virtual) ===");
        Shape2[] shapes = [ new Circle2(5), new Rectangle2(4,6), new Triangle2(3,8) ];
        foreach (var s in shapes)
            Console.WriteLine($"  {s.GetType().Name} area = {Area(s):F2}");
    }
}
