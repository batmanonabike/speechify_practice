using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex27_IComparableTests
{
    [Fact]
    public void SemanticVersion_CompareTo_SortsCorrectly()
    {
        var v100 = new SemanticVersion(1, 0, 0);
        var v110 = new SemanticVersion(1, 1, 0);
        var v111 = new SemanticVersion(1, 1, 1);
        Assert.True(v100 < v110);
        Assert.True(v110 < v111);
        Assert.True(v111 > v100);
    }

    [Fact]
    public void SemanticVersion_List_SortsAscending()
    {
        var versions = new[]
        {
            new SemanticVersion(2, 0, 0),
            new SemanticVersion(1, 0, 0),
            new SemanticVersion(1, 1, 0),
        };
        Array.Sort(versions);
        Assert.Equal(new SemanticVersion(1, 0, 0), versions[0]);
        Assert.Equal(new SemanticVersion(2, 0, 0), versions[2]);
    }
    [Fact]
    public void Card_CompareTo_SuitOrderFirst()
    {
        var clubs  = new Card(Suit.Clubs,   Rank.Ace);
        var spades = new Card(Suit.Spades,  Rank.Two);
        Assert.True(clubs.CompareTo(spades) < 0);
    }

    [Fact]
    public void Card_CompareTo_RankWithinSameSuit()
    {
        var two = new Card(Suit.Hearts, Rank.Two);
        var ace = new Card(Suit.Hearts, Rank.Ace);
        Assert.True(two.CompareTo(ace) < 0);
    }

    [Fact]
    public void CardSorter_SortAscending_LowestFirst()
    {
        var cards = new[]
        {
            new Card(Suit.Spades,   Rank.King),
            new Card(Suit.Clubs,    Rank.Two),
            new Card(Suit.Diamonds, Rank.Ten),
        };
        var sorted = CardSorter.SortAscending(cards);
        Assert.Equal(Suit.Clubs,    sorted[0].Suit);
        Assert.Equal(Suit.Spades,   sorted[2].Suit);
    }

    [Fact]
    public void CardSorter_SortDescending_HighestFirst()
    {
        var cards = new[]
        {
            new Card(Suit.Clubs,    Rank.Two),
            new Card(Suit.Spades,   Rank.Ace),
            new Card(Suit.Diamonds, Rank.Five),
        };
        var sorted = CardSorter.SortDescending(cards);
        Assert.Equal(Suit.Spades, sorted[0].Suit);
        Assert.Equal(Suit.Clubs,  sorted[2].Suit);
    }

    [Fact]
    public void Card_ToString_FormatsCorrectly()
    {
        var ace = new Card(Suit.Spades, Rank.Ace);
        Assert.NotEmpty(ace.ToString());
    }
}
