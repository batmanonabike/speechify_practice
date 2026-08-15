// Exercise 26 - IEquatable<T>
// Reference: docs/csharp-refresher/26_IEquatable.cs

namespace CSharpExercises;

/// <summary>
/// A monetary value.
/// Implement IEquatable<Money> with correct GetHashCode.
/// Two Money values are equal when both Currency and Amount match.
/// </summary>
public sealed class Money : IEquatable<Money>
{
    public string Currency { get; }
    public decimal Amount  { get; }

    public Money(string currency, decimal amount)
    { Currency = currency; Amount = amount; }

    public bool Equals(Money? other)   => throw new NotImplementedException();
    public override bool Equals(object? obj) => throw new NotImplementedException();
    public override int  GetHashCode()       => throw new NotImplementedException();

    // Also implement == and != operators
    public static bool operator ==(Money? a, Money? b) => throw new NotImplementedException();
    public static bool operator !=(Money? a, Money? b) => throw new NotImplementedException();
}

/// <summary>
