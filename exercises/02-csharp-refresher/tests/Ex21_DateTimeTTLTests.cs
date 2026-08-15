using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex21_DateTimeTTLTests
{
    [Fact]
    public void DaysBetween_ReturnsAbsoluteDays()
    {
        var a = new DateTimeOffset(2024, 1, 1, 0,0,0, TimeSpan.Zero);
        var b = new DateTimeOffset(2024, 1, 4, 0,0,0, TimeSpan.Zero);
        Assert.Equal(3, DateTimeExercises.DaysBetween(a, b));
        Assert.Equal(3, DateTimeExercises.DaysBetween(b, a)); // absolute
    }

    [Fact]
    public void IsWithinTtl_NotExpired_ReturnsTrue()
    {
        var now   = DateTimeOffset.UtcNow;
        var value = now - TimeSpan.FromMinutes(1);
        Assert.True(DateTimeExercises.IsWithinTtl(value, now, TimeSpan.FromMinutes(5)));
    }

    [Fact]
    public void IsWithinTtl_Expired_ReturnsFalse()
    {
        var now   = DateTimeOffset.UtcNow;
        var value = now - TimeSpan.FromMinutes(10);
        Assert.False(DateTimeExercises.IsWithinTtl(value, now, TimeSpan.FromMinutes(5)));
    }

    [Fact]
    public void TtlCacheEntry_NotExpired_TryGetValue_ReturnsTrue()
    {
        var now   = DateTimeOffset.UtcNow;
        var entry = new TtlCacheEntry<string>("hello", TimeSpan.FromMinutes(5), now);
        Assert.True(entry.TryGetValue(now + TimeSpan.FromMinutes(2), out var v));
        Assert.Equal("hello", v);
    }

    [Fact]
    public void TtlCacheEntry_Expired_TryGetValue_ReturnsFalse()
    {
        var now   = DateTimeOffset.UtcNow;
        var entry = new TtlCacheEntry<string>("hello", TimeSpan.FromMinutes(1), now);
        Assert.False(entry.TryGetValue(now + TimeSpan.FromMinutes(2), out _));
    }
}
