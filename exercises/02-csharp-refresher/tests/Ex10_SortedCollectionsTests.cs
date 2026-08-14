using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex10_SortedCollectionsTests
{
    [Fact]
    public void BuildLeaderboard_KeepsHighestScore()
    {
        var entries = new[] { ("cat", 5), ("dog", 3), ("cat", 9), ("ant", 2) };
        var board   = SortedCollectionExercises.BuildLeaderboard(entries);
        Assert.Equal(9, board["cat"]);
        Assert.Equal(3, board["dog"]);
        Assert.Equal(["ant","cat","dog"], board.Keys.ToList());
    }

    [Fact]
    public void KeyAtRank_ReturnsCorrectKey()
    {
        var sl = new SortedList<int,string> { [10]="a", [20]="b", [30]="c" };
        Assert.Equal(10, SortedCollectionExercises.KeyAtRank(sl, 0));
        Assert.Equal(30, SortedCollectionExercises.KeyAtRank(sl, 2));
    }

    [Fact]
    public void SortedDescendingUnique_DeduplicatesAndSortsDescending()
    {
        var result = SortedCollectionExercises.SortedDescendingUnique([3,1,2,3,2]).ToList();
        Assert.Equal([3,2,1], result);
    }
}
