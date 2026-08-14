using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex20_DITests
{
    // Minimal fakes — the user does not need to change these.
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
        var repo  = new FakeOrderRepo("ORD-1","ORD-2");
        var email = new FakeEmailService();
        var svc   = new OrderNotificationService(repo, email);

        await svc.NotifyAsync("cust1", "test@test.com");

        Assert.Equal("test@test.com",  email.LastTo);
        Assert.Equal("Order summary",  email.LastSubject);
        Assert.Contains("ORD-1",       email.LastBody);
        Assert.Contains("ORD-2",       email.LastBody);
    }

    [Fact]
    public async Task NotifyAsync_NoOrders_SendsNoOrdersEmail()
    {
        var repo  = new FakeOrderRepo();
        var email = new FakeEmailService();
        var svc   = new OrderNotificationService(repo, email);

        await svc.NotifyAsync("cust2", "empty@test.com");

        Assert.Equal("No orders",     email.LastSubject);
        Assert.Contains("No orders",  email.LastBody);
    }
}
