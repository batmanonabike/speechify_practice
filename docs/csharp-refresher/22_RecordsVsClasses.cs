// ============================================================
// Records vs Classes vs Structs
// ============================================================
//
// CLASS   — reference type; mutable by default; identity semantics
// RECORD  — reference type; immutable by default; VALUE semantics
//           (equality by content, not by reference)
// STRUCT  — value type; copied on assignment; stack-allocated (usually)
// RECORD STRUCT — value type + value semantics (C# 10+)
//
// CHOOSE
//   record       — DTOs, command/query objects, immutable data bags
//   class        — services, entities with identity, mutable state
//   struct       — small value objects (Point, Color, Money) with no
//                  inheritance and frequent copying
//   record struct— small value objects with content equality needed
// ============================================================

using System;

namespace CSharpRefresher;

// ============================================================
// 1. Record — immutable, value equality, with-expressions
// ============================================================
public record PaymentRequestRecord(
    decimal Amount,
    string  Currency,
    string  PaymentMethod);

// Derived record
public record InternationalPaymentRequest(
    decimal Amount,
    string  Currency,
    string  PaymentMethod,
    string  CountryCode)
    : PaymentRequestRecord(Amount, Currency, PaymentMethod);

// ============================================================
// 2. Class — reference type, reference equality by default
// ============================================================
public class PaymentRequestClass(decimal amount, string currency, string paymentMethod)
{
    public decimal Amount        { get; } = amount;
    public string  Currency      { get; } = currency;
    public string  PaymentMethod { get; } = paymentMethod;

    // Must manually implement equality for value semantics
}

// ============================================================
// 3. Mutable record — opt individual properties into mutability
// ============================================================
public record MutableConfig
{
    public string Environment { get; set; } = "Production";
    public int    TimeoutMs   { get; set; } = 5000;
}

// ============================================================
// 4. Struct — value type
// ============================================================
public struct MoneyStruct(decimal amount, string currency)
{
    public decimal Amount   { get; } = amount;
    public string  Currency { get; } = currency;

    public MoneyStruct Add(MoneyStruct other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Currency mismatch.");
        return new MoneyStruct(Amount + other.Amount, Currency);
    }

    public override string ToString() => $"{Amount} {Currency}";
}

// ============================================================
// 5. Record struct (C# 10+) — value type + content equality
// ============================================================
public readonly record struct Point(double X, double Y)
{
    public double DistanceTo(Point other) =>
        Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
}

// ============================================================
// 6. Positional vs non-positional record syntax
// ============================================================

// Positional: compiler generates constructor, Deconstruct, properties
public record PositionalAddress(string Street, string City, string PostCode);

// Non-positional: you write properties yourself; more control
public record NonPositionalAddress
{
    public string Street   { get; init; } = "";
    public string City     { get; init; } = "";
    public string PostCode { get; init; } = "";
}

public static class RecordsVsClassesExamples
{
    public static void Run()
    {
        // ============================================================
        // Value equality on records
        // ============================================================
        var r1 = new PaymentRequestRecord(100m, "USD", "card");
        var r2 = new PaymentRequestRecord(100m, "USD", "card");
        var r3 = new PaymentRequestRecord(200m, "USD", "card");

        Console.WriteLine($"r1 == r2 (same values): {r1 == r2}");       // True
        Console.WriteLine($"r1 == r3 (diff values): {r1 == r3}");       // False
        Console.WriteLine($"ReferenceEquals r1 r2:  {ReferenceEquals(r1, r2)}");  // False

        // ============================================================
        // Reference equality on classes (without override)
        // ============================================================
        var c1 = new PaymentRequestClass(100m, "USD", "card");
        var c2 = new PaymentRequestClass(100m, "USD", "card");
        Console.WriteLine($"\nc1 == c2 (class, same values): {c1 == c2}");  // False — different refs

        // ============================================================
        // with-expression — non-destructive mutation (records only)
        // ============================================================
        var original = new PaymentRequestRecord(100m, "USD", "card");
        var modified = original with { Amount = 200m };   // new record, original unchanged

        Console.WriteLine($"\nOriginal: {original}");
        Console.WriteLine($"Modified: {modified}");
        Console.WriteLine($"Same ref: {ReferenceEquals(original, modified)}");   // False

        // ============================================================
        // Deconstruct — positional records only
        // ============================================================
        var (amount, currency, method) = original;
        Console.WriteLine($"\nDeconstructed: {amount} {currency} {method}");

        // ============================================================
        // Inheritance — only for records
        // ============================================================
        var intl = new InternationalPaymentRequest(500m, "EUR", "bank_transfer", "DE");
        Console.WriteLine($"\nInternational: {intl}");
        Console.WriteLine($"Is PaymentRequestRecord: {intl is PaymentRequestRecord}");

        // ============================================================
        // Struct — copied on assignment
        // ============================================================
        var m1 = new MoneyStruct(10m, "USD");
        var m2 = m1;          // full copy
        Console.WriteLine($"\nm1: {m1}, m2: {m2}");
        Console.WriteLine($"m1 + m2: {m1.Add(m2)}");

        // ============================================================
        // Record struct — value equality + stack semantics
        // ============================================================
        var p1 = new Point(0, 0);
        var p2 = new Point(3, 4);
        var p3 = new Point(0, 0);

        Console.WriteLine($"\np1 == p3: {p1 == p3}");         // True — content equality
        Console.WriteLine($"Distance p1→p2: {p1.DistanceTo(p2):F4}");   // 5.0000

        // ============================================================
        // Mutable record — works but loses immutability guarantees
        // ============================================================
        var cfg = new MutableConfig { Environment = "Development", TimeoutMs = 1000 };
        cfg.TimeoutMs = 2000;   // allowed
        Console.WriteLine($"\nConfig: {cfg.Environment}, timeout={cfg.TimeoutMs}ms");

        // ============================================================
        // ToString — records generate it automatically
        // ============================================================
        Console.WriteLine($"\nAuto ToString: {r1}");
        // Output: PaymentRequestRecord { Amount = 100, Currency = USD, PaymentMethod = card }
    }
}
