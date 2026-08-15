// ============================================================
// IComparable<T>, IComparer<T>, and natural ordering
// ============================================================

using System;
using System.Collections.Generic;

namespace CSharpRefresher;

public sealed class Version2 : IEquatable<Version2>, IComparable<Version2>
{
    public int Major { get; }
    public int Minor { get; }
    public int Patch { get; }

    public Version2(int major, int minor, int patch)
    {
        Major = major; Minor = minor; Patch = patch;
    }

    public int CompareTo(Version2? other)
    {
        if (other is null) return 1;
        int comparison = Major.CompareTo(other.Major);
        if (comparison != 0) return comparison;
        comparison = Minor.CompareTo(other.Minor);
        return comparison != 0 ? comparison : Patch.CompareTo(other.Patch);
    }

    public bool Equals(Version2? other) =>
        other is not null && CompareTo(other) == 0;

    public override bool Equals(object? obj) => Equals(obj as Version2);
    public override int GetHashCode() => HashCode.Combine(Major, Minor, Patch);

    public static bool operator ==(Version2? left, Version2? right) => Equals(left, right);
    public static bool operator !=(Version2? left, Version2? right) => !Equals(left, right);
    public static bool operator <(Version2 left, Version2 right) => left.CompareTo(right) < 0;
    public static bool operator >(Version2 left, Version2 right) => left.CompareTo(right) > 0;
    public static bool operator <=(Version2 left, Version2 right) => left.CompareTo(right) <= 0;
    public static bool operator >=(Version2 left, Version2 right) => left.CompareTo(right) >= 0;

    public override string ToString() => $"{Major}.{Minor}.{Patch}";
}

public sealed class MoneyByAmountComparer : IComparer<Money2>
{
    public static readonly MoneyByAmountComparer Instance = new();

    public int Compare(Money2? x, Money2? y)
    {
        if (x is null && y is null) return 0;
        if (x is null) return -1;
        if (y is null) return 1;
        return x.Amount.CompareTo(y.Amount);
    }
}

public static class ComparableExamples
{
    public static void Run()
    {
        var v1 = new Version2(1, 0, 0);
        var v2 = new Version2(1, 2, 0);
        var v3 = new Version2(2, 0, 0);

        Console.WriteLine($"v1 < v2: {v1 < v2}");
        Console.WriteLine($"v3 > v2: {v3 > v2}");

        var versions = new List<Version2> { v3, v1, v2 };
        versions.Sort();
        Console.WriteLine("Sorted: " + string.Join(", ", versions));

        var prices = new List<Money2>
        {
            new(30m, "USD"),
            new(10m, "EUR"),
            new(20m, "GBP"),
        };
        prices.Sort(MoneyByAmountComparer.Instance);
        Console.WriteLine("By amount: " + string.Join(", ", prices));
    }
}
