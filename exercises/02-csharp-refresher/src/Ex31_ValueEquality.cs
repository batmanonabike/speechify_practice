// Exercise 31 — Value equality across all type kinds
// Reference: docs/csharp-refresher/32_ValueEquality_AllCases.cs

namespace CSharpExercises;

// ---------------------------------------------------------------
// Your task: implement each type so that all tests pass.
// ---------------------------------------------------------------

/// <summary>
/// CLASS with value equality.
/// Two instances are equal when Currency AND Amount match.
/// Must implement IEquatable<PriceClass>, override Equals(object?),
/// GetHashCode, and == / != operators.
/// </summary>
public class PriceClass : IEquatable<PriceClass>
{
    public string  Currency { get; }
    public decimal Amount   { get; }
    public PriceClass(string currency, decimal amount)
    { Currency = currency; Amount = amount; }

    public bool Equals(PriceClass? other)        => throw new NotImplementedException();
    public override bool Equals(object? obj)     => throw new NotImplementedException();
    public override int  GetHashCode()           => throw new NotImplementedException();
    public static bool operator ==(PriceClass? a, PriceClass? b) => throw new NotImplementedException();
    public static bool operator !=(PriceClass? a, PriceClass? b) => throw new NotImplementedException();
}

/// <summary>
/// RECORD — value equality is automatic.
/// Just define the record; confirm you understand that == works out-of-the-box.
/// Add a Discounted(decimal pct) method that returns a new record with
/// Amount reduced by pct% using a with-expression.
/// </summary>
public record PriceRecord(string Currency, decimal Amount)
{
    public PriceRecord Discounted(decimal pct)
        => throw new NotImplementedException();
}

/// <summary>
/// STRUCT — value equality via IEquatable<T>.
/// Two instances are equal when both fields match.
/// Structs should implement IEquatable for performance (avoids boxing).
/// </summary>
public struct PriceStruct : IEquatable<PriceStruct>
{
    public string  Currency { get; init; }
    public decimal Amount   { get; init; }

    public bool Equals(PriceStruct other)    => throw new NotImplementedException();
    public override bool Equals(object? obj) => throw new NotImplementedException();
    public override int  GetHashCode()       => throw new NotImplementedException();
}

/// <summary>
/// RECORD STRUCT — value equality is automatic (same as record).
/// Just define it; add a WithTax(decimal rate) method that returns a
/// new record struct with Amount * (1 + rate).
/// </summary>
public record struct PriceRecordStruct(string Currency, decimal Amount)
{
    public PriceRecordStruct WithTax(decimal rate)
        => throw new NotImplementedException();
}
