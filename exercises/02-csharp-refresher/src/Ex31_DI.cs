// Exercise 31 - Constructor Injection & Microsoft.Extensions.DependencyInjection
// Reference: docs/csharp-refresher/31_DependencyInjection.cs
//
// Part A: implement OrderNotificationService using constructor injection.
// Part B: wire everything up with a real ServiceCollection / ServiceProvider.

using Microsoft.Extensions.DependencyInjection;

namespace CSharpExercises;

// ---------------------------------------------------------------
// Contracts — do not change.
// ---------------------------------------------------------------

public interface IOrderRepository
{
    Task<IEnumerable<string>> GetOrderIdsAsync(string customerId);
}

public interface IEmailService
{
    Task SendAsync(string to, string subject, string body);
}

// ---------------------------------------------------------------
// Part A — your task: implement the service using constructor injection.
// ---------------------------------------------------------------

/// <summary>
/// OrderNotificationService retrieves order IDs from IOrderRepository and
/// sends an email summary via IEmailService.
///
/// Constructor: accept IOrderRepository and IEmailService (store as private fields).
///
/// NotifyAsync(customerId, email):
///   1. Fetch order IDs for customerId.
///   2. If none, send email with subject "No orders" and body "No orders found."
///   3. Otherwise send subject "Order summary" and body listing each ID on its own line.
/// </summary>
public class OrderNotificationService
{
    private readonly IOrderRepository _repo;
    private readonly IEmailService _email;

    public OrderNotificationService(IOrderRepository repo, IEmailService email)
    { _repo = repo; _email = email; }

    public Task NotifyAsync(string customerId, string email)
        => throw new NotImplementedException();
}

// ---------------------------------------------------------------
// Part B — concrete implementations to register with the container.
// ---------------------------------------------------------------

/// <summary>
/// A real (stub) order repository you will register as the IOrderRepository impl.
/// For this exercise it always returns two fake order IDs.
/// </summary>
public class InMemoryOrderRepository : IOrderRepository
{
    public Task<IEnumerable<string>> GetOrderIdsAsync(string customerId)
        => throw new NotImplementedException();
    // Hint: return Task.FromResult<IEnumerable<string>>(new[] { "ORD-001", "ORD-002" });
}

/// <summary>
/// A real (stub) email service you will register as the IEmailService impl.
/// Store the last sent message in public properties so tests can inspect it.
/// </summary>
public class StubEmailService : IEmailService
{
    public string? LastTo      { get; private set; }
    public string? LastSubject { get; private set; }
    public string? LastBody    { get; private set; }

    public Task SendAsync(string to, string subject, string body)
        => throw new NotImplementedException();
    // Hint: set the three properties, return Task.CompletedTask
}

// ---------------------------------------------------------------
// Part B — your task: build and use a ServiceCollection.
// ---------------------------------------------------------------

/// <summary>
/// Wire up the DI container and resolve OrderNotificationService from it.
///
/// Task: complete BuildServiceProvider() so that:
///   • IOrderRepository  → InMemoryOrderRepository  (Transient)
///   • IEmailService     → StubEmailService          (Singleton)
///   • OrderNotificationService                      (Transient)
///
/// Then complete ResolveAndNotifyAsync() to:
///   1. Call BuildServiceProvider().
///   2. Resolve an OrderNotificationService from the provider.
///   3. Call NotifyAsync("cust1", "test@example.com").
///   4. Return the StubEmailService so the caller can inspect LastSubject etc.
///
/// Hint: provider.GetRequiredService&lt;T&gt;()
/// </summary>
public static class ContainerExercise
{
    public static ServiceProvider BuildServiceProvider()
        => throw new NotImplementedException();

    public static async Task<StubEmailService> ResolveAndNotifyAsync()
        => throw new NotImplementedException();
}

