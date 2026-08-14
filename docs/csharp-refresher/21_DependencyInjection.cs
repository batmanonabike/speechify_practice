// ============================================================
// Dependency Injection — concepts and patterns
// ============================================================
// DI = supplying an object's dependencies from outside rather
// than letting it create them itself.
//
// WHY
//   - Loose coupling: depend on abstractions (interfaces), not concretions
//   - Testability: swap real dependencies for fakes/mocks in tests
//   - SOLID: supports D (Dependency Inversion) and O (Open/Closed)
//
// HOW (without a container — pure constructor injection)
//   1. Define dependency as an interface
//   2. Accept it in the constructor
//   3. Caller (composition root) wires everything up
// ============================================================

using System;
using System.Collections.Generic;

namespace CSharpRefresher;

// ============================================================
// 1. The problem without DI
// ============================================================
public class BadOrderService
{
    // Creates its own dependency — untestable, tightly coupled
    private readonly List<string> _log = [];

    public void PlaceOrder(string item)
    {
        // Can't swap this out in tests or for different environments
        Console.WriteLine($"[ConsoleLog] Order placed: {item}");
        _log.Add(item);
    }
}

// ============================================================
// 2. Define abstractions
// ============================================================
public interface ILogger2            // named 2 to avoid clash with 12_Inheritance_Interfaces
{
    void Log(string message);
}

public interface IOrderRepository
{
    void Save(string orderId, string item);
    IEnumerable<string> GetAll();
}

public interface INotificationService
{
    void Notify(string recipient, string message);
}

// ============================================================
// 3. Concrete implementations
// ============================================================
public class ConsoleLogger2 : ILogger2
{
    public void Log(string message) => Console.WriteLine($"[LOG] {message}");
}

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly Dictionary<string, string> _store = [];

    public void Save(string orderId, string item)
    {
        _store[orderId] = item;
    }

    public IEnumerable<string> GetAll() => _store.Values;
}

public class EmailNotificationService(ILogger2 logger) : INotificationService
{
    public void Notify(string recipient, string message)
    {
        logger.Log($"Sending email to {recipient}: {message}");
        // real impl would call SMTP here
    }
}

// ============================================================
// 4. Service using constructor injection
// ============================================================
public class OrderService(
    IOrderRepository repository,
    ILogger2 logger,
    INotificationService notifications)
{
    public void PlaceOrder(string orderId, string item, string customerEmail)
    {
        logger.Log($"Placing order {orderId} for {item}");
        repository.Save(orderId, item);
        notifications.Notify(customerEmail, $"Your order {orderId} has been placed.");
        logger.Log($"Order {orderId} complete.");
    }

    public IEnumerable<string> GetAllOrders() => repository.GetAll();
}

// ============================================================
// 5. Composition root — where everything is wired up
//    In real apps this is your Program.cs / Startup.cs.
//    In tests you inject fakes instead.
// ============================================================
public static class CompositionRoot
{
    public static OrderService Build()
    {
        var logger       = new ConsoleLogger2();
        var repo         = new InMemoryOrderRepository();
        var notification = new EmailNotificationService(logger);
        return new OrderService(repo, logger, notification);
    }
}

// ============================================================
// 6. Fake implementations for testing
//    No mocking framework needed for simple cases.
// ============================================================
public class FakeLogger : ILogger2
{
    public List<string> Entries { get; } = [];
    public void Log(string message) => Entries.Add(message);
}

public class FakeOrderRepository : IOrderRepository
{
    private readonly Dictionary<string, string> _store = [];
    public void Save(string orderId, string item) => _store[orderId] = item;
    public IEnumerable<string> GetAll() => _store.Values;
}

public class FakeNotificationService : INotificationService
{
    public List<(string Recipient, string Message)> Sent { get; } = [];
    public void Notify(string recipient, string message) => Sent.Add((recipient, message));
}

// ============================================================
// 7. Optional dependencies & null object pattern
// ============================================================
public class NullLogger : ILogger2
{
    // Does nothing — safe default when logging is optional
    public void Log(string message) { }
}

// ============================================================
// 8. Decorator via DI — add behaviour by wrapping
// ============================================================
public class TimedOrderRepository(IOrderRepository inner, ILogger2 logger) : IOrderRepository
{
    public void Save(string orderId, string item)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        inner.Save(orderId, item);
        sw.Stop();
        logger.Log($"Repository.Save took {sw.ElapsedMilliseconds}ms");
    }

    public IEnumerable<string> GetAll() => inner.GetAll();
}

public static class DependencyInjectionExamples
{
    public static void Run()
    {
        Console.WriteLine("=== Real composition ===");
        var realService = CompositionRoot.Build();
        realService.PlaceOrder("ORD-001", "Widget", "alice@example.com");
        realService.PlaceOrder("ORD-002", "Gadget", "bob@example.com");
        Console.WriteLine("All orders: " + string.Join(", ", realService.GetAllOrders()));

        Console.WriteLine("\n=== Test composition (fakes) ===");
        var fakeLogger   = new FakeLogger();
        var fakeRepo     = new FakeOrderRepository();
        var fakeNotifier = new FakeNotificationService();
        var testService  = new OrderService(fakeRepo, fakeLogger, fakeNotifier);

        testService.PlaceOrder("ORD-003", "Sprocket", "carol@example.com");

        Console.WriteLine($"Log entries: {fakeLogger.Entries.Count}");
        Console.WriteLine($"Notifications sent: {fakeNotifier.Sent.Count}");
        Console.WriteLine($"Recipient: {fakeNotifier.Sent[0].Recipient}");

        Console.WriteLine("\n=== Decorated repository (via DI) ===");
        var logger  = new ConsoleLogger2();
        var repo    = new InMemoryOrderRepository();
        var timed   = new TimedOrderRepository(repo, logger);   // wrap with timing decorator
        var notifications = new EmailNotificationService(logger);
        var service = new OrderService(timed, logger, notifications);  // inject decorated repo

        service.PlaceOrder("ORD-004", "Doohickey", "dave@example.com");
    }
}
