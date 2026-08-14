// ============================================================
// Date & Time processing — and TTL patterns
// ============================================================
// KEY TYPES
//   DateTime        — a point in time (date + time), no timezone info
//   DateTimeOffset  — a point in time WITH UTC offset — prefer this
//   TimeSpan        — a duration / interval
//   DateOnly        — date without time (C# 10+)
//   TimeOnly        — time without date (C# 10+)
//   TimeZoneInfo    — timezone conversion
//
// GOLDEN RULE
//   Store and compare in UTC.
//   Convert to local time only for display.
//   Prefer DateTimeOffset over DateTime for anything crossing timezones.
// ============================================================

using System;
using System.Collections.Generic;
using System.Globalization;

namespace CSharpRefresher;

public static class DateTimeExamples
{
    public static void Run()
    {
        // ============================================================
        // 1. CREATING DateTime / DateTimeOffset
        // ============================================================
        DateTime now        = DateTime.UtcNow;                    // always use UTC
        DateTime local      = DateTime.Now;                       // local — avoid in server code
        DateTime today      = DateTime.Today;                     // midnight local — avoid
        DateTime specific   = new DateTime(2026, 8, 14, 9, 30, 0, DateTimeKind.Utc);
        DateTime epoch      = DateTime.UnixEpoch;                 // 1970-01-01 UTC

        DateTimeOffset dtoNow  = DateTimeOffset.UtcNow;          // preferred over DateTime.UtcNow
        DateTimeOffset dtoSpec = new DateTimeOffset(2026, 8, 14, 9, 30, 0, TimeSpan.Zero);

        Console.WriteLine($"UTC now:     {now:O}");               // O = ISO 8601 round-trip format
        Console.WriteLine($"DTO now:     {dtoNow:O}");
        Console.WriteLine($"Specific:    {specific:yyyy-MM-dd HH:mm:ss}");

        // ============================================================
        // 2. TimeSpan — durations
        // ============================================================
        TimeSpan fiveMinutes  = TimeSpan.FromMinutes(5);
        TimeSpan oneHour      = TimeSpan.FromHours(1);
        TimeSpan oneDay       = TimeSpan.FromDays(1);
        TimeSpan combined     = TimeSpan.FromHours(1) + TimeSpan.FromMinutes(30); // 1h 30m

        TimeSpan literal      = new TimeSpan(hours: 2, minutes: 15, seconds: 0);

        Console.WriteLine($"\n5 min in seconds: {fiveMinutes.TotalSeconds}");
        Console.WriteLine($"1.5 hours:        {combined}");             // 01:30:00
        Console.WriteLine($"TotalMinutes:     {combined.TotalMinutes}"); // 90

        // ============================================================
        // 3. ARITHMETIC — adding and subtracting
        // ============================================================
        DateTime future  = now.Add(fiveMinutes);
        DateTime past    = now.Subtract(oneDay);
        DateTime nextWk  = now.AddDays(7);
        DateTime nextMth = now.AddMonths(1);
        DateTime nextYr  = now.AddYears(1);

        TimeSpan elapsed = now - past;              // subtract two DateTimes → TimeSpan
        Console.WriteLine($"\nElapsed since yesterday: {elapsed.TotalHours:F1}h");
        Console.WriteLine($"Next week: {nextWk:yyyy-MM-dd}");

        // ============================================================
        // 4. COMPARISON
        // ============================================================
        bool isBefore = now < future;
        bool isAfter  = now > past;
        int  cmp      = DateTime.Compare(now, future);   // negative = now is earlier

        // Safe: always compare UTC to UTC
        bool expired  = now > specific.AddMinutes(5);

        Console.WriteLine($"\nisBefore: {isBefore}, isAfter: {isAfter}");

        // ============================================================
        // 5. FORMATTING & PARSING
        // ============================================================
        string iso        = now.ToString("O");                            // 2026-08-14T09:30:00.0000000Z
        string readable   = now.ToString("yyyy-MM-dd HH:mm:ss");
        string custom     = now.ToString("ddd dd MMM yyyy", CultureInfo.InvariantCulture);
        string shortDate  = now.ToShortDateString();                      // locale-dependent

        Console.WriteLine($"\nISO 8601:  {iso}");
        Console.WriteLine($"Readable:  {readable}");
        Console.WriteLine($"Custom:    {custom}");

        // Parsing
        DateTime parsed  = DateTime.Parse("2026-08-14T09:30:00Z");
        DateTime parsedU = DateTime.Parse("2026-08-14T09:30:00Z", null, DateTimeStyles.RoundtripKind);

        bool ok = DateTime.TryParse("not-a-date", out DateTime safe);   // false
        bool ok2 = DateTime.TryParseExact(
            "14/08/2026",
            "dd/MM/yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out DateTime exact);

        Console.WriteLine($"TryParse ok: {ok}, TryParseExact ok: {ok2}, exact={exact:yyyy-MM-dd}");

        // DateTimeOffset parsing
        DateTimeOffset dto = DateTimeOffset.Parse("2026-08-14T09:30:00+01:00");
        Console.WriteLine($"DTO parsed UTC: {dto.UtcDateTime:O}");

        // ============================================================
        // 6. DateOnly & TimeOnly (C# 10+)
        // ============================================================
        DateOnly dateOnly = DateOnly.FromDateTime(now);
        TimeOnly timeOnly = TimeOnly.FromDateTime(now);

        DateOnly birthday    = new DateOnly(1990, 6, 15);
        TimeOnly meeting     = new TimeOnly(14, 30);           // 14:30

        int age = dateOnly.Year - birthday.Year;
        if (dateOnly < birthday.AddYears(age)) age--;          // birthday not yet this year

        Console.WriteLine($"\nDateOnly: {dateOnly}, TimeOnly: {timeOnly:HH:mm}");
        Console.WriteLine($"Meeting: {meeting}, Age: {age}");

        // ============================================================
        // 7. TIMEZONE CONVERSION
        // ============================================================
        DateTime utc = DateTime.UtcNow;

        // Convert UTC → specific timezone
        TimeZoneInfo eastern  = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        DateTime easternTime  = TimeZoneInfo.ConvertTimeFromUtc(utc, eastern);
        Console.WriteLine($"\nUTC:     {utc:HH:mm}");
        Console.WriteLine($"Eastern: {easternTime:HH:mm}");

        // DateTimeOffset preserves the original offset
        DateTimeOffset withOffset = new DateTimeOffset(utc, TimeSpan.Zero);
        DateTimeOffset converted  = TimeZoneInfo.ConvertTime(withOffset, eastern);
        Console.WriteLine($"DTO Eastern: {converted:O}");

        // ============================================================
        // 8. UNIX TIMESTAMPS
        // ============================================================
        long   unixSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long   unixMs      = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        DateTimeOffset fromUnix = DateTimeOffset.FromUnixTimeSeconds(unixSeconds);
        Console.WriteLine($"\nUnix seconds: {unixSeconds}");
        Console.WriteLine($"Back to DTO:  {fromUnix:O}");

        // ============================================================
        // 9. TTL — TIME-TO-LIVE PATTERNS
        // ============================================================
        Console.WriteLine("\n=== TTL patterns ===");

        // --- Pattern 1: ExpiresAt timestamp (used in CachedCurrencyRateClient) ---
        // Store the absolute expiry time alongside the value.
        // On read: check DateTime.UtcNow < ExpiresAt.
        var ttl         = TimeSpan.FromMinutes(5);
        var cachedAt    = DateTime.UtcNow;
        var expiresAt   = cachedAt.Add(ttl);

        bool isValid    = DateTime.UtcNow < expiresAt;
        bool isExpired  = DateTime.UtcNow >= expiresAt;
        Console.WriteLine($"Cache valid: {isValid}, Expired: {isExpired}");

        // --- Pattern 2: Age check (how long since something was set?) ---
        var storedAt    = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(6));
        var age2        = DateTime.UtcNow - storedAt;
        bool tooOld     = age2 > ttl;
        Console.WriteLine($"Age: {age2.TotalMinutes:F1} min, TooOld: {tooOld}");

        // --- Pattern 3: Sliding expiry (reset on each access) ---
        var slidingTtl  = TimeSpan.FromMinutes(5);
        DateTime lastAccessed = DateTime.UtcNow;

        void RecordAccess()  => lastAccessed = DateTime.UtcNow;   // call on each cache hit
        bool IsSlidingExpired() => DateTime.UtcNow - lastAccessed > slidingTtl;

        RecordAccess();   // simulate a cache hit resetting the sliding window

        Console.WriteLine($"Sliding expired (just accessed): {IsSlidingExpired()}");

        // --- Pattern 4: TTL with IClock (testable — mirrors the exercise) ---
        var clock       = new FakeClockForDates(new DateTime(2026, 8, 14, 9, 0, 0, DateTimeKind.Utc));
        var entry       = new TtlEntry<string>("hello", clock.UtcNow, TimeSpan.FromMinutes(5));

        Console.WriteLine($"Entry valid at t=0: {entry.IsValid(clock.UtcNow)}");   // true

        clock.Advance(TimeSpan.FromMinutes(4));
        Console.WriteLine($"Entry valid at t=4m: {entry.IsValid(clock.UtcNow)}");  // true

        clock.Advance(TimeSpan.FromMinutes(2));
        Console.WriteLine($"Entry valid at t=6m: {entry.IsValid(clock.UtcNow)}");  // false — expired

        // --- Pattern 5: Scheduling — "run at" vs "run after" ---
        DateTime runAt     = DateTime.UtcNow.AddMinutes(10);
        TimeSpan runAfter  = TimeSpan.FromMinutes(10);

        // "run at" — compare against UtcNow
        bool shouldRun     = DateTime.UtcNow >= runAt;

        // "run after" — track elapsed from a start point
        DateTime startedAt = DateTime.UtcNow;
        bool durationMet   = (DateTime.UtcNow - startedAt) >= runAfter;

        Console.WriteLine($"ShouldRun: {shouldRun}, DurationMet: {durationMet}");

        // ============================================================
        // 10. COMMON PITFALLS
        // ============================================================

        // PITFALL 1: DateTime.Kind mismatch — UTC vs Unspecified vs Local
        DateTime utcTime  = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);  // wrong! sets kind but doesn't convert
        DateTime correct  = DateTime.UtcNow;                                        // right
        // Always use UtcNow. Never use SpecifyKind to "fix" a DateTime.

        // PITFALL 2: Comparing DateTime from different timezones without normalization
        // Fix: always subtract to UTC before comparing
        DateTime a = DateTime.UtcNow;
        DateTime b = DateTime.Now;  // local time — different Kind
        // bool bad = a < b;       // compares raw values ignoring Kind — BUG
        bool good = a < b.ToUniversalTime();  // convert b first

        // PITFALL 3: Month arithmetic — months have different lengths
        DateTime endOfJan = new DateTime(2026, 1, 31, 0, 0, 0, DateTimeKind.Utc);
        DateTime nextMonth = endOfJan.AddMonths(1);     // 2026-02-28, not 2026-03-03
        Console.WriteLine($"\nJan 31 + 1 month = {nextMonth:yyyy-MM-dd}");

        // PITFALL 4: Stopwatch for elapsed time, not DateTime subtraction
        var sw = System.Diagnostics.Stopwatch.StartNew();
        // ... do work ...
        sw.Stop();
        Console.WriteLine($"Elapsed (Stopwatch): {sw.ElapsedMilliseconds}ms");
        // DateTime subtraction is affected by DST changes and clock adjustments.
        // Use Stopwatch for measuring code performance.
    }
}

// ============================================================
// Supporting types for TTL pattern demos
// ============================================================

public interface IClockForDates
{
    DateTime UtcNow { get; }
}

public class FakeClockForDates(DateTime initial) : IClockForDates
{
    public DateTime UtcNow { get; private set; } = initial;
    public void Advance(TimeSpan by) => UtcNow = UtcNow.Add(by);
}

public class SystemClockForDates : IClockForDates
{
    public DateTime UtcNow => DateTime.UtcNow;
}

// Reusable TTL entry — generic, testable via IClockForDates
public sealed class TtlEntry<T>(T value, DateTime createdAt, TimeSpan ttl)
{
    public T        Value     { get; } = value;
    public DateTime ExpiresAt { get; } = createdAt.Add(ttl);

    public bool IsValid(DateTime utcNow)  => utcNow < ExpiresAt;
    public bool IsExpired(DateTime utcNow) => !IsValid(utcNow);
    public TimeSpan Remaining(DateTime utcNow) => ExpiresAt - utcNow;
}
