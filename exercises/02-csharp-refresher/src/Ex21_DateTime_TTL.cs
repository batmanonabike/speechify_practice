// Exercise 21 - DateTime, DateTimeOffset, TTL patterns
// Reference: docs/csharp-refresher/21_DateTime_TTL.cs

namespace CSharpExercises;

public static class DateTimeExercises
{
    /// <summary>
    /// Return the number of COMPLETE days between two dates (absolute difference).
    /// e.g. Jan 1 → Jan 3  = 2 days.
    /// </summary>
    public static int DaysBetween(DateTimeOffset a, DateTimeOffset b)
        => (int)(b - a).Duration().TotalDays;

    /// <summary>
    /// Given a UTC DateTimeOffset, convert it to the specified
    /// <paramref name="timeZoneId"/> (e.g. "Eastern Standard Time")
    /// using TimeZoneInfo.
    /// </summary>
    public static DateTimeOffset ConvertToZone(DateTimeOffset utc, string timeZoneId)
    {
        ArgumentNullException.ThrowIfNull(timeZoneId);
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        return TimeZoneInfo.ConvertTime(utc, timeZone);
    }

    /// <summary>
    /// Return true if <paramref name="value"/> was recorded within the last
    /// <paramref name="ttl"/> from <paramref name="now"/>.
    /// i.e. (now - value) <= ttl
    /// </summary>
    public static bool IsWithinTtl(DateTimeOffset value, DateTimeOffset now, TimeSpan ttl)
    {
        return (now - value) <= ttl;
    }
}

/// <summary>
/// A generic TTL cache entry.
/// Implement IsExpired(now) and TryGetValue(now, out T value).
/// </summary>
public class TtlCacheEntry<T>
{
    public T Value { get; }
    public DateTimeOffset ExpiresAt { get; }

    public TtlCacheEntry(T value, TimeSpan ttl, DateTimeOffset now)
    {
        Value = value;
        ExpiresAt = now + ttl;
    }

    /// <summary>Return true if now >= ExpiresAt.</summary>
    public bool IsExpired(DateTimeOffset now) => now >= ExpiresAt;
    
    /// <summary>
    /// If not expired, set <paramref name="value"/> and return true.
    /// Otherwise set default and return false.
    /// </summary>
    public bool TryGetValue(DateTimeOffset now, out T value)
    {
        if (!IsExpired(now))
        {
            value = Value;
            return true;
        }

        value = default!;
        return false;
    }
}
