// Exercise 27 - IComparable<T>, IComparer<T>, and sorting
// Reference: docs/csharp-refresher/27_IComparable.cs

namespace CSharpExercises;

/// <summary>
/// A simple semantic version: Major.Minor.Patch.
/// Implement IComparable so versions sort lowest-to-highest.
/// </summary>
public sealed class SemanticVersion : IComparable<SemanticVersion>
{
    public int Major { get; }
    public int Minor { get; }
    public int Patch { get; }

    public SemanticVersion(int major, int minor, int patch)
    { Major = major; Minor = minor; Patch = patch; }

    public int CompareTo(SemanticVersion? other) => throw new NotImplementedException();

    public static bool operator <(SemanticVersion a, SemanticVersion b) => throw new NotImplementedException();
    public static bool operator >(SemanticVersion a, SemanticVersion b) => throw new NotImplementedException();
    public static bool operator <=(SemanticVersion a, SemanticVersion b) => throw new NotImplementedException();
    public static bool operator >=(SemanticVersion a, SemanticVersion b) => throw new NotImplementedException();
    public static bool operator ==(SemanticVersion a, SemanticVersion b) => throw new NotImplementedException();
    public static bool operator !=(SemanticVersion a, SemanticVersion b) => throw new NotImplementedException();

    public override bool Equals(object? obj) => throw new NotImplementedException();
    public override int GetHashCode() => throw new NotImplementedException();
}

/// <summary>
/// A playing card that is naturally comparable:
/// Suit order: Clubs < Diamonds < Hearts < Spades
/// Within a suit, order by Rank (2–14 where 14=Ace).
/// Implement IComparable<Card> and override ToString() → e.g. "A♠".
/// </summary>
public enum Suit  { Clubs, Diamonds, Hearts, Spades }
public enum Rank  { Two=2,Three,Four,Five,Six,Seven,Eight,Nine,Ten,Jack=11,Queen=12,King=13,Ace=14 }

public sealed class Card : IComparable<Card>
{
    public Suit Suit { get; }
    public Rank Rank { get; }

    public Card(Suit suit, Rank rank) { Suit = suit; Rank = rank; }

    public int CompareTo(Card? other) => throw new NotImplementedException();
    public override string ToString() => throw new NotImplementedException();
}

/// <summary>
/// An IComparer<Card> that reverses the natural order (highest card first).
/// </summary>
public class CardDescendingComparer : IComparer<Card>
{
    public int Compare(Card? x, Card? y) => throw new NotImplementedException();
}

public static class CardSorter
{
    /// <summary>Sort cards using their natural (ascending) order.</summary>
    public static List<Card> SortAscending(IEnumerable<Card> cards)
        => throw new NotImplementedException();

    /// <summary>Sort cards using CardDescendingComparer.</summary>
    public static List<Card> SortDescending(IEnumerable<Card> cards)
        => throw new NotImplementedException();
}
