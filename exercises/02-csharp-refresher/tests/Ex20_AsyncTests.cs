using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex20_AsyncTests
{
    [Fact]
    public async Task FetchUserNameAsync_ValidId_ReturnsName()
    {
        var name = await AsyncExercises.FetchUserNameAsync(42);
        Assert.Equal("User_42", name);
    }

    [Fact]
    public async Task FetchUserNameAsync_ZeroId_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(
            () => AsyncExercises.FetchUserNameAsync(0));
    }

    [Fact]
    public async Task FetchAllAsync_ReturnsNamesInOrder()
    {
        var names = await AsyncExercises.FetchAllAsync([1, 2, 3]);
        Assert.Equal(["User_1","User_2","User_3"], names);
    }

    [Fact]
    public async Task WithTimeoutAsync_CompletesInTime_ReturnsResult()
    {
        var result = await AsyncExercises.WithTimeoutAsync(
            async ct => { await Task.Delay(10, ct); return 99; }, 500);
        Assert.Equal(99, result);
    }

    [Fact]
    public async Task WithTimeoutAsync_Exceeds_ThrowsCancelled()
    {
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => AsyncExercises.WithTimeoutAsync(
                async ct => { await Task.Delay(1000, ct); return 0; }, 50));
    }

    [Fact]
    public async Task SumWhereAsync_SumsMatchingItems()
    {
        async IAsyncEnumerable<int> Source()
        {
            foreach (var i in Enumerable.Range(1, 10))
            {
                await Task.Yield();
                yield return i;
            }
        }
        var sum = await AsyncExercises.SumWhereAsync(Source(), x => x % 2 == 0);
        Assert.Equal(30, sum); // 2+4+6+8+10
    }
}
