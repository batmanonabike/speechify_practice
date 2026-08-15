// Exercise 17 - Polymorphism
// Reference: docs/csharp-refresher/17_Polymorphism.cs

namespace CSharpExercises;

// ---------------------------------------------------------------
// Base type — do not change.
// ---------------------------------------------------------------
public abstract class Notification
{
    public string Recipient { get; }
    public string Message   { get; }

    protected Notification(string recipient, string message)
    { Recipient = recipient; Message = message; }

    /// Override in each subclass to return the delivery channel, e.g. "Email".
    public abstract string Channel();

    /// Virtual — default returns "[{Channel}] To:{Recipient}: {Message}".
    public virtual string Format() =>
        $"[{Channel()}] To:{Recipient}: {Message}";
}

// ---------------------------------------------------------------
// Your task: implement the three concrete notifications.
// ---------------------------------------------------------------

/// <summary>Channel = "Email". No override of Format needed.</summary>
public class EmailNotification : Notification
{
    public EmailNotification(string recipient, string message)
        : base(recipient, message) { }

    public override string Channel() => throw new NotImplementedException();
}

/// <summary>
/// Channel = "SMS".
/// Override Format to truncate Message to 160 chars if longer.
/// Format: "[SMS] To:{Recipient}: {truncated message}"
/// </summary>
public class SmsNotification : Notification
{
    public SmsNotification(string recipient, string message)
        : base(recipient, message) { }

    public override string Channel() => throw new NotImplementedException();
    public override string Format()  => throw new NotImplementedException();
}

/// <summary>
/// Channel = "Push".
/// No override of Format needed.
/// Add a Priority property (int, set in constructor, default 0).
/// </summary>
public class PushNotification : Notification
{
    public int Priority { get; }

    public PushNotification(string recipient, string message, int priority = 0)
        : base(recipient, message)
        => Priority = priority;

    public override string Channel() => throw new NotImplementedException();
}

/// <summary>
/// Dispatch a list of notifications, returning only those whose Channel()
/// matches <paramref name="channel"/> (case-insensitive).
/// Hint: virtual dispatch means you just call n.Channel() — no casting needed.
/// </summary>
public static class NotificationDispatcher
{
    public static IEnumerable<Notification> FilterByChannel(
        IEnumerable<Notification> notifications, string channel)
        => throw new NotImplementedException();
}
