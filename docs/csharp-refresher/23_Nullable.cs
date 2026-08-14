// ============================================================
// Nullable Reference Types & null-safety patterns
// ============================================================
// Enabled with: <Nullable>enable</Nullable> in .csproj
// (already on in this project)
//
// RULES
//   string  — non-nullable; compiler warns if null can flow in
//   string? — explicitly nullable; must null-check before use
//   !       — null-forgiving operator (suppresses warning — use sparingly)
//   ??      — null-coalescing: return right-hand side if left is null
//   ??=     — null-coalescing assignment
//   ?.      — null-conditional: short-circuits to null if left is null
//   ?[]     — null-conditional indexer
// ============================================================

using System;
using System.Collections.Generic;

namespace CSharpRefresher;

public static class NullableExamples
{
    // ============================================================
    // 1. Nullable reference type annotations
    // ============================================================
    private static int GetLength(string? value)
    {
        if (value is null) return 0;
        return value.Length;    // compiler knows value is not null here
    }

    private static string Greet(string? name)
    {
        // ?? provides a fallback
        return $"Hello, {name ?? "stranger"}!";
    }

    // ============================================================
    // 2. Null-conditional operator ?.
    // ============================================================
    private record Address(string Street, string? City);
    private record Customer(string Name, Address? Address);

    private static string GetCity(Customer? customer)
    {
        // Without ?. you'd need nested null checks
        return customer?.Address?.City ?? "Unknown";
    }

    // ============================================================
    // 3. Null-coalescing assignment ??=
    // ============================================================
    private static readonly Dictionary<string, List<string>> _groups = [];

    private static void AddToGroup(string key, string value)
    {
        _groups[key] ??= [];           // initialise if absent
        _groups[key].Add(value);
    }

    // ============================================================
    // 4. Null-forgiving operator ! — use sparingly
    // ============================================================
    private static string? _initialised = null;

    private static void Initialise() => _initialised = "ready";

    private static void UseInitialised()
    {
        Initialise();
        // We KNOW it's set here but the compiler can't prove it.
        // Use ! to suppress the warning — document WHY it's safe.
        Console.WriteLine(_initialised!.ToUpper());
    }

    // ============================================================
    // 5. Nullable value types (predates NRT — still important)
    // ============================================================
    private static double? SafeDivide(double numerator, double denominator)
    {
        if (denominator == 0) return null;
        return numerator / denominator;
    }

    private static void NullableValueTypeDemo()
    {
        double? result = SafeDivide(10, 3);
        Console.WriteLine(result.HasValue ? $"Result: {result.Value:F4}" : "Division by zero");

        double? zero = SafeDivide(10, 0);
        Console.WriteLine(zero ?? -1);       // -1 fallback

        // Lifted operators — arithmetic on nullable propagates null
        int? a = 5;
        int? b = null;
        int? c = a + b;   // null — any null operand makes result null
        Console.WriteLine($"5 + null = {c?.ToString() ?? "null"}");

        // GetValueOrDefault
        Console.WriteLine(b.GetValueOrDefault(0));   // 0
    }

    // ============================================================
    // 6. Pattern matching with null
    // ============================================================
    private static string Classify(object? obj) => obj switch
    {
        null          => "null",
        int n when n < 0 => "negative int",
        int n         => $"int: {n}",
        string s      => $"string: \"{s}\"",
        _             => $"other: {obj.GetType().Name}"
    };

    // ============================================================
    // 7. Guard clauses — fail fast at boundaries
    // ============================================================
    public static string ProcessName(string? name)
    {
        // ArgumentNullException.ThrowIfNull (NET 6+)
        ArgumentNullException.ThrowIfNull(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return name.Trim().ToUpper();
    }

    // ============================================================
    // 8. Returning null vs empty vs Optional
    // ============================================================
    private static List<int> GetNumbers(bool returnNull)
    {
        // Prefer returning empty collection over null
        if (returnNull) return [];
        return [1, 2, 3];
    }

    public static void Run()
    {
        Console.WriteLine("=== Nullable reference types ===");
        Console.WriteLine(GetLength(null));       // 0
        Console.WriteLine(GetLength("hello"));    // 5
        Console.WriteLine(Greet(null));            // Hello, stranger!
        Console.WriteLine(Greet("Alice"));         // Hello, Alice!

        Console.WriteLine("\n=== Null-conditional ===");
        var customer1 = new Customer("Alice", new Address("1 Main St", "London"));
        var customer2 = new Customer("Bob",   new Address("2 High St", null));
        var customer3 = new Customer("Carol", null);
        Customer? customer4 = null;

        Console.WriteLine(GetCity(customer1));   // London
        Console.WriteLine(GetCity(customer2));   // Unknown
        Console.WriteLine(GetCity(customer3));   // Unknown
        Console.WriteLine(GetCity(customer4));   // Unknown

        Console.WriteLine("\n=== ??= ===");
        AddToGroup("fruits", "apple");
        AddToGroup("fruits", "banana");
        AddToGroup("vegs",   "carrot");
        foreach (var (k, v) in _groups)
            Console.WriteLine($"{k}: {string.Join(", ", v)}");

        Console.WriteLine("\n=== Nullable value types ===");
        NullableValueTypeDemo();

        Console.WriteLine("\n=== Pattern matching with null ===");
        foreach (object? o in new object?[] { null, -3, 42, "hi", 3.14 })
            Console.WriteLine(Classify(o));

        Console.WriteLine("\n=== Guard clauses ===");
        Console.WriteLine(ProcessName("  alice  "));
        try { ProcessName(null); }
        catch (ArgumentNullException ex) { Console.WriteLine($"Caught: {ex.GetType().Name}"); }
        try { ProcessName("  "); }
        catch (ArgumentException ex) { Console.WriteLine($"Caught: {ex.GetType().Name}"); }

        Console.WriteLine("\n=== Empty vs null collections ===");
        var list = GetNumbers(returnNull: true);
        Console.WriteLine($"Count: {list.Count}");   // 0 — safe to iterate, no null check needed
    }
}
