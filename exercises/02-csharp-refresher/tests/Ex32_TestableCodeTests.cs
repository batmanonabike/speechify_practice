using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

// Fake implementations of the repository interface for test isolation.
file class InMemoryUserRepository : IUserRepository
{
    private readonly Dictionary<int,string> _store = new();
    public Task<string?> FindByIdAsync(int id)
        => Task.FromResult(_store.TryGetValue(id, out var v) ? v : null);
    public Task SaveAsync(int id, string name)
    {
        _store[id] = name;
        return Task.CompletedTask;
    }
}

public class Ex32_TestableCodeTests
{
    private static (UserService svc, IUserRepository repo) Build()
    {
        var repo = new InMemoryUserRepository();
        return (new UserService(repo), repo);
    }

    [Fact]
    public async Task GetUserNameAsync_ExistingUser_ReturnsName()
    {
        var (svc, repo) = Build();
        await repo.SaveAsync(1, "Alice");
        Assert.Equal("Alice", await svc.GetUserNameAsync(1));
    }

    [Fact]
    public async Task GetUserNameAsync_MissingUser_ThrowsKeyNotFound()
    {
        var (svc, _) = Build();
        await Assert.ThrowsAsync<KeyNotFoundException>(() => svc.GetUserNameAsync(99));
    }

    [Fact]
    public async Task CreateUserAsync_EmptyName_ThrowsArgumentException()
    {
        var (svc, _) = Build();
        await Assert.ThrowsAsync<ArgumentException>(() => svc.CreateUserAsync(1, ""));
    }

    [Fact]
    public async Task CreateUserAsync_ValidName_CanBeRetrieved()
    {
        var (svc, _) = Build();
        await svc.CreateUserAsync(2, "Bob");
        Assert.Equal("Bob", await svc.GetUserNameAsync(2));
    }
}
