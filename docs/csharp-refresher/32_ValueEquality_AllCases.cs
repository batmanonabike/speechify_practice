// ============================================================
// Value equality — all four type kinds side-by-side
// ============================================================
//
//  TYPE            DEFAULT ==          VALUE EQUALITY HOW?
//  ─────────────   ─────────────────   ──────────────────────────────────────
//  class           reference (bad)     implement IEquatable<T> manually
//  record          value  ✓ (free)     compiler generates it — nothing to do
//  struct          value* (slow)       implement IEquatable<T> manually
//  record struct   value  ✓ (free)     compiler generates it — nothing to do
//
//  * struct default Equals uses reflection — correct but allocates + slow.
//    Always implement IEquatable<T> on structs you compare frequently.
//
// CONTRACT (must hold for all implementations):
//   1.  a.Equals(a)                           → true   (reflexive)
//   2.  a.Equals(b) == b.Equals(a)            → true   (symmetric)
//   3.  if a==b && b==c then a==c             → true   (transitive)
//   4.  a.Equals(b) → a.GetHashCode()==b.GetHashCode()  (hash consistency)
//   5.  GetHashCode() must not change while object is in a hash collection
// ============================================================

using System;
using System.Collections.Generic;

namespace CSharpRefresher;

// ============================================================
// 1. CLASS — reference equality by default (broken for value semantics)
//    Fix: implement IEquatable<T> + override Equals + GetHashCode + operators
// ============================================================
public sealed class CurrencyAmount : IEquatable<CurrencyAmount>
{
    public decimal Amount   { get; }
    public string  Currency { get; }

    public CurrencyAmount(decimal amount, string currency)
    {
        Amount   = amount;
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));
    }

    // 1. Typed Equals — avoids boxing, used by generic collections
    public bool Equals(CurrencyAmount? other)
    {
        if (other is null)                 return false;
        if (ReferenceEquals(this, other))  return true;   // same ref → definitely equal
        return Amount == other.Amount &&
               string.Equals(Currency, other.Currency, StringComparison.OrdinalIgnoreCase);
    }

    // 2. Object Equals — delegates to typed version
    public override bool Equals(object? obj) => Equals(obj as CurrencyAmount);

    // 3. GetHashCode — MUST be consistent with Equals
    //    Use only immutable fields. HashCode.Combine is the modern way.
    public override int GetHashCode() =>
        HashCode.Combine(Amount, Currency.ToUpperInvariant());

    // 4. Operators — use object.Equals static for null-safe dispatch
    public static bool operator ==(CurrencyAmount? a, CurrencyAmount? b) =>
        ReferenceEquals(a, b) || (a is not null && a.Equals(b));

    public static bool operator !=(CurrencyAmount? a, CurrencyAmount? b) => !(a == b);

    public override string ToString() => $"{Amount} {Currency}";
}

// ============================================================
// 2. RECORD — value equality is FREE (compiler generates everything)
//    Generated: Equals(T?), Equals(object?), GetHashCode, ==, !=, ToString, Deconstruct
//    Nothing extra required.
// ============================================================
public record CurrencyAmountRecord(decimal Amount, string Currency);

// ============================================================
// 3. STRUCT — value* equality by default, but SLOW (reflection)
//    Fix: implement IEquatable<T> + override Equals + GetHashCode + operators
//    Same pattern as class but no null checks on 'this'.
// ============================================================
public readonly struct CurrencyAmountStruct : IEquatable<CurrencyAmountStruct>
{
    public decimal Amount   { get; }
    public string  Currency { get; }

    public CurrencyAmountStruct(decimal amount, string currency)
    {
        Amount   = amount;
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));
    }

    // 1. Typed Equals — no null check on 'this' (structs can't be null)
    public bool Equals(CurrencyAmountStruct other) =>
        Amount == other.Amount &&
        string.Equals(Currency, other.Currency, StringComparison.OrdinalIgnoreCase);

    // 2. Object Equals — box-free fast path via pattern match
    public override bool Equals(object? obj) =>
        obj is CurrencyAmountStruct other && Equals(other);

    // 3. GetHashCode
    public override int GetHashCode() =>
        HashCode.Combine(Amount, Currency.ToUpperInvariant());

    // 4. Operators
    public static bool operator ==(CurrencyAmountStruct a, CurrencyAmountStruct b) => a.Equals(b);
    public static bool operator !=(CurrencyAmountStruct a, CurrencyAmountStruct b) => !a.Equals(b);

    public override string ToString() => $"{Amount} {Currency}";
}

// ============================================================
// 4. RECORD STRUCT — value equality is FREE (compiler generates everything)
//    Like record but a value type. Use 'readonly' to make it immutable.
//    Nothing extra required.
// ============================================================
public readonly record struct CurrencyAmountRecordStruct(decimal Amount, string Currency);

// ============================================================
// QUICK SUMMARY TABLE (as executable demo)
// ============================================================
public static class ValueEqualityExamples
{
    private static void PrintResult(string label, bool result, bool expected)
    {
        string status = result == expected ? "✓" : "✗ WRONG";
        Console.WriteLine($"  {label,-55} = {result} {status}");
    }

    public static void Run()
    {
        // ---- 1. CLASS ----
        Console.WriteLine("=== class (IEquatable<T> implemented) ===");
        var c1 = new CurrencyAmount(10m, "USD");
        var c2 = new CurrencyAmount(10m, "usd");   // different case
        var c3 = new CurrencyAmount(20m, "USD");
        var c4 = c1;                               // same reference

        // Capture hashes before == calls so nullable flow analysis stays happy
        int c1Hash = c1.GetHashCode();
        int c2Hash = c2.GetHashCode();

        PrintResult("c1 == c2  (same value, diff case)",    c1 == c2,           true);
        PrintResult("c1 == c3  (different value)",          c1 == c3,           false);
        PrintResult("c1 == c4  (same reference)",           c1 == c4,           true);
        PrintResult("c1.Equals(null)",                      c1.Equals((CurrencyAmount?)null), false);
        PrintResult("c1.GetHashCode() == c2.GetHashCode()", c1Hash == c2Hash,   true);

        // Verify Dictionary and HashSet work correctly
        var key = new CurrencyAmount(10m, "USD");
        var dict = new Dictionary<CurrencyAmount, string> { [key] = "ten" };
        Console.WriteLine($"  Dict lookup with new equal key: {dict[new CurrencyAmount(10m, "USD")]}");

        var set = new HashSet<CurrencyAmount> { key, new CurrencyAmount(10m, "usd"), new CurrencyAmount(20m, "USD") };
        Console.WriteLine($"  HashSet count (c1==c2, so 2 unique): {set.Count}");

        // ---- 2. RECORD ----
        Console.WriteLine("\n=== record (value equality — free) ===");
        var r1 = new CurrencyAmountRecord(10m, "USD");
        var r2 = new CurrencyAmountRecord(10m, "USD");
        var r3 = new CurrencyAmountRecord(20m, "USD");

        PrintResult("r1 == r2  (same value)",               r1 == r2,           true);
        PrintResult("r1 == r3  (different value)",          r1 == r3,           false);
        PrintResult("ReferenceEquals(r1, r2)",              ReferenceEquals(r1, r2), false);
        PrintResult("r1.GetHashCode() == r2.GetHashCode()", r1.GetHashCode() == r2.GetHashCode(), true);

        // with-expression produces a new record with changed field
        var r4 = r1 with { Amount = 20m };
        PrintResult("r1 == r4  (after with-expression)",    r1 == r4,           false);
        PrintResult("r3 == r4  (same values via with)",     r3 == r4,           true);

        // ---- 3. STRUCT ----
        Console.WriteLine("\n=== struct (IEquatable<T> implemented) ===");
        var s1 = new CurrencyAmountStruct(10m, "USD");
        var s2 = new CurrencyAmountStruct(10m, "usd");
        var s3 = new CurrencyAmountStruct(20m, "USD");

        PrintResult("s1 == s2  (same value, diff case)",    s1 == s2,           true);
        PrintResult("s1 == s3  (different value)",          s1 == s3,           false);
        PrintResult("s1.GetHashCode() == s2.GetHashCode()", s1.GetHashCode() == s2.GetHashCode(), true);

        // Structs are copied on assignment — equality still works
        var s4 = s1;
        s4 = new CurrencyAmountStruct(99m, "EUR");     // s1 unchanged
        PrintResult("s1 unaffected after copy+mutate",      s1.Amount == 10m,   true);

        // ---- 4. RECORD STRUCT ----
        Console.WriteLine("\n=== readonly record struct (value equality — free) ===");
        var rs1 = new CurrencyAmountRecordStruct(10m, "USD");
        var rs2 = new CurrencyAmountRecordStruct(10m, "USD");
        var rs3 = new CurrencyAmountRecordStruct(20m, "USD");

        PrintResult("rs1 == rs2 (same value)",              rs1 == rs2,         true);
        PrintResult("rs1 == rs3 (different value)",         rs1 == rs3,         false);
        PrintResult("rs1.GetHashCode()==rs2.GetHashCode()", rs1.GetHashCode() == rs2.GetHashCode(), true);

        var rs4 = rs1 with { Amount = 20m };
        PrintResult("rs3 == rs4 (same values via with)",    rs3 == rs4,         true);

        // ---- SUMMARY ----
        Console.WriteLine("""

        ┌──────────────────┬────────────────┬──────────────────────────┐
        │ Type             │ Default ==     │ For value equality        │
        ├──────────────────┼────────────────┼──────────────────────────┤
        │ class            │ reference      │ implement IEquatable<T>  │
        │ record           │ value  ✓       │ nothing — free           │
        │ struct           │ value* (slow)  │ implement IEquatable<T>  │
        │ readonly record  │ value  ✓       │ nothing — free           │
        │   struct         │                │                          │
        └──────────────────┴────────────────┴──────────────────────────┘
        * struct default uses reflection: correct but allocates & slow
        """);
    }
}
