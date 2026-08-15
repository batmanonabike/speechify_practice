using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex17_PolymorphismTests
{
    [Fact]
    public void EmailNotification_Channel_IsEmail()
    {
        var n = new EmailNotification("a@b.com", "hello");
        Assert.Equal("Email", n.Channel());
    }

    [Fact]
    public void SmsNotification_Format_TruncatesLongMessage()
    {
        var longMsg = new string('x', 200);
        var n       = new SmsNotification("07700", longMsg);
        Assert.True(n.Format().Contains(new string('x', 160)));
        Assert.DoesNotContain(new string('x', 161), n.Format());
    }

    [Fact]
    public void SmsNotification_Format_DoesNotTruncateShortMessage()
    {
        var n = new SmsNotification("07700", "Hi!");
        Assert.Contains("Hi!", n.Format());
    }

    [Fact]
    public void PushNotification_DefaultPriority_IsZero()
    {
        var n = new PushNotification("dev1", "ping");
        Assert.Equal(0, n.Priority);
    }

    [Fact]
    public void FilterByChannel_ReturnsOnlyMatchingChannel()
    {
        var notifications = new Notification[]
        {
            new EmailNotification("a","msg"),
            new SmsNotification("b","msg"),
            new PushNotification("c","msg"),
        };
        var emails = NotificationDispatcher.FilterByChannel(notifications, "email").ToList();
        Assert.Single(emails);
        Assert.IsType<EmailNotification>(emails[0]);
    }
}
