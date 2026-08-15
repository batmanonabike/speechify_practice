// ============================================================
// IEquatable<T>, equality, and GetHashCode
// ============================================================
// IEquatable<T>  — strongly-typed equality; avoids boxing
//
// RULES
//   1. If you override Equals(object) you MUST override GetHashCode().
//   2. Objects that are Equal MUST have the same hash code.
//   3. GetHashCode() must be stable for the lifetime of the object
//      (don't include mutable fields used in collections).
//   4. Implement IEquatable<T> on structs to avoid boxing.
//   5. records implement all of this automatically.
// ============================================================

using System;
using System.Collections.Generic;

namespace CSharpRefresher;

// ============================================================
// 1. Class — manual equality (without IEquatable)
//    Shows WHY IEquatable is needed
// ============================================================
public class MoneyBad(decimal amount, string currency)
{
    public decimal Amount   { get; } = amount;
    public string  Currency { get; } = currency;

    // Without overriding Equals: reference equality only
    // new MoneyBad(10, "USD") == new MoneyBad(10, "USD") → FALSE
}

// ============================================================
// 2. Class — correct IEquatable<T> implementation
// ============================================================
public sealed class Money2 : IEquatable<Money2>
{
    public decimal Amount   { get; }
    public string  Currency { get; }

    public Money2(decimal amount, string currency)
    {
        Amount   = amount;
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));
    }

    // IEquatable<T> — strongly typed, no boxing
    public bool Equals(Money2? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Amount == other.Amount &&
               string.Equals(Currency, other.Currency, StringComparison.OrdinalIgnoreCase);
    }

    // Override object.Equals — calls the typed version
    public override bool Equals(object? obj) => Equals(obj as Money2);

    // MUST override when overriding Equals
    public override int GetHashCode() =>
        HashCode.Combine(Amount, Currency.ToUpperInvariant());

    // Operator overloads — expected when you have value equality
    public static bool operator ==(Money2? left, Money2? right)  => Equals(left, right);
    public static bool operator !=(Money2? left, Money2? right)  => !Equals(left, right);

    public override string ToString() => $"{Amount} {Currency}";
}

// ============================================================
// 3. Struct — IEquatable<T> avoids boxing
// ============================================================
// Structs get a default Equals via reflection (slow + boxes).
// Always implement IEquatable<T> on structs you compare frequently.
public readonly struct Temperature2 : IEquatable<Temperature2>
{
    public double  Value { get; }
    public char    Scale { get; }   // 'C', 'F', 'K'

    public Temperature2(double value, char scale) { Value = value; Scale = scale; }

    public bool Equals(Temperature2 other) =>
        Value == other.Value && Scale == other.Scale;

    public override bool Equals(object? obj) =>
        obj is Temperature2 t && Equals(t);

    public override int GetHashCode() => HashCode.Combine(Value, Scale);

    public static bool operator ==(Temperature2 a, Temperature2 b) => a.Equals(b);
    public static bool operator !=(Temperature2 a, Temperature2 b) => !a.Equals(b);

    public override string ToString() => $"{Value}°{Scale}";
}

// ============================================================
// 4. record — gets all of this for free
// ============================================================
// Compiler generates: Equals, GetHashCode, ==, !=, ToString, Deconstruct
public record ProductRecord(string Sku, decimal Price);

public static class EquatableExamples
{
    public static void Run()
    {
        // ---- Class without IEquatable ----
        Console.WriteLine("=== Class without IEquatable ===");
        var b1 = new MoneyBad(10m, "USD");
        var b2 = new MoneyBad(10m, "USD");
        Console.WriteLine($"b1 == b2 (ref): {b1 == b2}");      // False — reference equality

        // ---- IEquatable<T> on class ----
        Console.WriteLine("\n=== IEquatable on class ===");
        var m1 = new Money2(10m, "USD");
        var m2 = new Money2(10m, "usd");   // different case — should still be equal
        var m3 = new Money2(20m, "USD");

        Console.WriteLine($"m1 == m2 (same amount, diff case): {m1 == m2}");  // True
        Console.WriteLine($"m1 == m3 (diff amount):            {m1 == m3}");  // False
        Console.WriteLine($"m1.Equals(m2): {m1.Equals(m2)}");                 // True
        Console.WriteLine($"m1.GetHashCode() == m2.GetHashCode(): {m1.GetHashCode() == m2.GetHashCode()}"); // True

        // Works correctly as a Dictionary key
        var wallet = new Dictionary<Money2, string>
        {
            [new Money2(10m, "USD")] = "ten dollars"
        };
        Console.WriteLine($"Dict lookup: {wallet[new Money2(10m, "usd")]}");  // "ten dollars"

        // Works correctly in HashSet
        var set = new HashSet<Money2> { m1, m2, m3 };
        Console.WriteLine($"HashSet count (m1==m2 so 2 unique): {set.Count}");  // 2

        // ---- Struct IEquatable — no boxing ----
        Console.WriteLine("\n=== IEquatable on struct ===");
        var t1 = new Temperature2(100, 'C');
        var t2 = new Temperature2(100, 'C');
        var t3 = new Temperature2(212, 'F');

        Console.WriteLine($"t1 == t2: {t1 == t2}");   // True
        Console.WriteLine($"t1 == t3: {t1 == t3}");   // False (different scale)

        // ---- record — free equality ----
        Console.WriteLine("\n=== record equality (free) ===");
        var p1 = new ProductRecord("SKU-1", 9.99m);
        var p2 = new ProductRecord("SKU-1", 9.99m);
        var p3 = new ProductRecord("SKU-2", 4.99m);

        Console.WriteLine($"p1 == p2: {p1 == p2}");   // True  — value equality
        Console.WriteLine($"p1 == p3: {p1 == p3}");   // False
        Console.WriteLine($"p1.GetHashCode() == p2.GetHashCode(): {p1.GetHashCode() == p2.GetHashCode()}");

        var productSet = new HashSet<ProductRecord> { p1, p2, p3 };
        Console.WriteLine($"HashSet count: {productSet.Count}");  // 2 — p1 and p2 are duplicates

        // ---- GetHashCode pitfall — mutable fields ----
        Console.WriteLine("\n=== GetHashCode pitfall ===");
        // Never include mutable state in GetHashCode if the object will be used in a
        // Dictionary or HashSet — mutating a key corrupts the collection.
        // Safe: use only immutable / readonly fields in GetHashCode.
        Console.WriteLine("(see comments in source)");
    }
}
