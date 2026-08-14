// Exercise 20 — Dependency Injection & interfaces
// Reference: docs/csharp-refresher/20_DI.cs

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
// Your task: implement the service that uses both dependencies.
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
