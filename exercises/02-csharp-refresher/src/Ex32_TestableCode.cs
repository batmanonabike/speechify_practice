// Exercise 32 - Unit testing patterns (test helpers, not the tests themselves)
// Reference: docs/csharp-refresher/32_UnitTestingPatterns.cs
//
// NOTE: the actual xUnit test files are in the tests/ project.
// This file contains the production code you will test.

namespace CSharpExercises;

/// <summary>
/// A simple in-memory repository for demonstration.
/// </summary>
public interface IUserRepository
{
    Task<string?> FindByIdAsync(int id);
    Task SaveAsync(int id, string name);
}

/// <summary>
/// Service that wraps IUserRepository with some business logic.
/// Your task: implement the methods.
/// </summary>
public class UserService
{
    private readonly IUserRepository _repo;
    public UserService(IUserRepository repo) => _repo = repo;

    /// <summary>
    /// Return the user name, or throw KeyNotFoundException if not found.
    /// </summary>
    public async Task<string> GetUserNameAsync(int id)
        => throw new NotImplementedException();

    /// <summary>
    /// Save the user only if the name is non-empty; otherwise throw ArgumentException.
    /// </summary>
    public async Task CreateUserAsync(int id, string name)
        => throw new NotImplementedException();
}
