using CSharpExercises;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex31_DITests
{
    // ---------------------------------------------------------------
    // Part A — constructor injection with hand-crafted fakes
    // ---------------------------------------------------------------

    private class FakeOrderRepo : IOrderRepository
    {
        private readonly string[] _ids;
        public FakeOrderRepo(params string[] ids) => _ids = ids;
        public Task<IEnumerable<string>> GetOrderIdsAsync(string _)
            => Task.FromResult<IEnumerable<string>>(_ids);
    }

    private class FakeEmailService : IEmailService
    {
        public string? LastTo      { get; private set; }
        public string? LastSubject { get; private set; }
        public string? LastBody    { get; private set; }
        public Task SendAsync(string to, string subject, string body)
        {
            LastTo = to; LastSubject = subject; LastBody = body;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task NotifyAsync_WithOrders_SendsSummaryEmail()
    {
        var repo  = new FakeOrderRepo("ORD-1", "ORD-2");
        var email = new FakeEmailService();
        var svc   = new OrderNotificationService(repo, email);

        await svc.NotifyAsync("cust1", "test@test.com");

        Assert.Equal("test@test.com", email.LastTo);
        Assert.Equal("Order summary", email.LastSubject);
        Assert.Contains("ORD-1",      email.LastBody);
        Assert.Contains("ORD-2",      email.LastBody);
    }

    [Fact]
    public async Task NotifyAsync_NoOrders_SendsNoOrdersEmail()
    {
        var repo  = new FakeOrderRepo();
        var email = new FakeEmailService();
        var svc   = new OrderNotificationService(repo, email);

        await svc.NotifyAsync("cust2", "empty@test.com");

        Assert.Equal("No orders",    email.LastSubject);
        Assert.Contains("No orders", email.LastBody);
    }

    // ---------------------------------------------------------------
    // Part B — wiring via ServiceCollection / ServiceProvider
    // ---------------------------------------------------------------

    [Fact]
    public void BuildServiceProvider_CanResolveOrderNotificationService()
    {
        // Your ContainerExercise.BuildServiceProvider() must register
        // IOrderRepository, IEmailService, and OrderNotificationService.
        using var provider = ContainerExercise.BuildServiceProvider();
        var svc = provider.GetRequiredService<OrderNotificationService>();
        Assert.NotNull(svc);
    }

    [Fact]
    public void BuildServiceProvider_IEmailService_IsSingleton()
    {
        // StubEmailService is registered as Singleton — same instance each resolve.
        using var provider = ContainerExercise.BuildServiceProvider();
        var a = provider.GetRequiredService<IEmailService>();
        var b = provider.GetRequiredService<IEmailService>();
        Assert.Same(a, b);
    }

    [Fact]
    public void BuildServiceProvider_OrderNotificationService_IsTransient()
    {
        // OrderNotificationService is registered as Transient — new instance each resolve.
        using var provider = ContainerExercise.BuildServiceProvider();
        var a = provider.GetRequiredService<OrderNotificationService>();
        var b = provider.GetRequiredService<OrderNotificationService>();
        Assert.NotSame(a, b);
    }

    [Fact]
    public async Task ResolveAndNotifyAsync_SendsOrderSummaryEmail()
    {
        // Full end-to-end: container resolves the service, service sends the email.
        var emailSvc = await ContainerExercise.ResolveAndNotifyAsync();
        Assert.Equal("Order summary", emailSvc.LastSubject);
        Assert.NotNull(emailSvc.LastBody);
    }
}

