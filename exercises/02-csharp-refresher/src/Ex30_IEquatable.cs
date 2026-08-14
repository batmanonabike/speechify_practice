// Exercise 30 — IEquatable<T> and IComparable<T>
// Reference: docs/csharp-refresher/31_IEquatable.cs

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
/// A simple semantic version: Major.Minor.Patch.
/// Implement IComparable<SemanticVersion> so that versions sort
/// lowest-to-highest (Major first, then Minor, then Patch).
/// Also implement the comparison operators: <, >, <=, >=, ==, !=.
/// </summary>
public sealed class SemanticVersion : IComparable<SemanticVersion>
{
    public int Major { get; }
    public int Minor { get; }
    public int Patch { get; }

    public SemanticVersion(int major, int minor, int patch)
    { Major = major; Minor = minor; Patch = patch; }

    public int CompareTo(SemanticVersion? other) => throw new NotImplementedException();

    public static bool operator < (SemanticVersion a, SemanticVersion b) => throw new NotImplementedException();
    public static bool operator > (SemanticVersion a, SemanticVersion b) => throw new NotImplementedException();
    public static bool operator <=(SemanticVersion a, SemanticVersion b) => throw new NotImplementedException();
    public static bool operator >=(SemanticVersion a, SemanticVersion b) => throw new NotImplementedException();
    public static bool operator ==(SemanticVersion a, SemanticVersion b) => throw new NotImplementedException();
    public static bool operator !=(SemanticVersion a, SemanticVersion b) => throw new NotImplementedException();

    public override bool Equals(object? obj) => throw new NotImplementedException();
    public override int  GetHashCode()       => throw new NotImplementedException();
}
