// ============================================================
// Extension Methods
// ============================================================
// Extension methods add new methods to an existing type without
// modifying it, subclassing it, or recompiling it.
//
// RULES
//   - Must be in a static class
//   - Must be static methods
//   - First parameter uses `this` keyword — that's the type being extended
//   - Called as if they were instance methods on the target type
//   - Cannot access private members of the target type
//   - Lower priority than instance methods with the same signature
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpRefresher;

// ============================================================
// 1. Extending string
// ============================================================
public static class StringExtensions
{
    // Truncate with optional ellipsis
    public static string Truncate(this string value, int maxLength, string suffix = "...")
    {
        if (value.Length <= maxLength) return value;
        return value[..(maxLength - suffix.Length)] + suffix;
    }

    // Convert to title case
    public static string ToTitleCase(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return value;
        return string.Join(" ", value.Split(' ')
            .Select(w => w.Length > 0 ? char.ToUpper(w[0]) + w[1..].ToLower() : w));
    }

    // Null-safe contains (string? receiver)
    public static bool ContainsIgnoreCase(this string? value, string search)
        => value?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false;

    // Repeat a string n times
    public static string Repeat(this string value, int count)
        => new StringBuilder(value.Length * count).Insert(0, value, count).ToString();

    // Parse to int safely
    public static int? ToIntOrNull(this string? value)
        => int.TryParse(value, out int result) ? result : null;
}

// ============================================================
// 2. Extending IEnumerable<T>
// ============================================================
public static class EnumerableExtensions
{
    // Batch/chunk — split into pages
    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int size)
    {
        var batch = new List<T>(size);
        foreach (var item in source)
        {
            batch.Add(item);
            if (batch.Count == size)
            {
                yield return batch;
                batch = new List<T>(size);
            }
        }
        if (batch.Count > 0) yield return batch;
    }

    // ForEach — execute an action on each element
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source) action(item);
    }

    // Null-safe Any
    public static bool SafeAny<T>(this IEnumerable<T>? source) => source?.Any() ?? false;

    // Flatten one level of nesting
    public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source)
        => source.SelectMany(x => x);

    // Pick a random element
    public static T RandomElement<T>(this IList<T> source, Random? rng = null)
    {
        if (source.Count == 0) throw new InvalidOperationException("Collection is empty.");
        return source[(rng ?? Random.Shared).Next(source.Count)];
    }
}

// ============================================================
// 3. Extending your own domain types
// ============================================================
public record Money(decimal Amount, string Currency);

public static class MoneyExtensions
{
    public static Money Add(this Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Currency mismatch.");
        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public static Money ApplyTax(this Money money, decimal taxRate)
        => money with { Amount = Math.Round(money.Amount * (1 + taxRate), 2) };

    public static bool IsZero(this Money money)  => money.Amount == 0m;
    public static bool IsPositive(this Money money) => money.Amount > 0m;

    public static string Display(this Money money) => $"{money.Amount:N2} {money.Currency}";
}

// ============================================================
// 4. Extending interfaces — applies to ALL implementors
// ============================================================
public interface IHasTimestamp
{
    DateTime CreatedAt { get; }
}

public static class TimestampExtensions
{
    public static bool IsOlderThan(this IHasTimestamp item, TimeSpan age)
        => DateTime.UtcNow - item.CreatedAt > age;

    public static string Age(this IHasTimestamp item)
    {
        var span = DateTime.UtcNow - item.CreatedAt;
        return span.TotalDays >= 1 ? $"{(int)span.TotalDays}d ago"
             : span.TotalHours >= 1 ? $"{(int)span.TotalHours}h ago"
             : $"{(int)span.TotalMinutes}m ago";
    }
}

public record AuditedEntity(string Name, DateTime CreatedAt) : IHasTimestamp;

// ============================================================
// 5. Extension on nullable types
// ============================================================
public static class NullableExtensions
{
    public static T OrThrow<T>(this T? value, string message) where T : class
        => value ?? throw new InvalidOperationException(message);

    public static TOut? MapNullable<TIn, TOut>(this TIn? value, Func<TIn, TOut> transform)
        where TIn : class
        where TOut : class
        => value is null ? null : transform(value);
}

public static class ExtensionMethodsExamples
{
    public static void Run()
    {
        Console.WriteLine("=== String extensions ===");
        string text = "The quick brown fox";
        Console.WriteLine(text.Truncate(12));            // "The quick..."
        Console.WriteLine("hello world".ToTitleCase());  // "Hello World"
        Console.WriteLine("Hello".Repeat(3));            // "HelloHelloHello"
        Console.WriteLine("42".ToIntOrNull());            // 42
        Console.WriteLine("abc".ToIntOrNull());           // null
        Console.WriteLine(((string?)null).ContainsIgnoreCase("test"));  // False

        Console.WriteLine("\n=== Enumerable extensions ===");
        var numbers = Enumerable.Range(1, 10).ToList();

        foreach (var batch in numbers.Batch(3))
            Console.WriteLine("  batch: " + string.Join(",", batch));

        numbers.ForEach(n => Console.Write(n + " "));
        Console.WriteLine();

        var nested = new[] { new[] {1,2}, new[] {3,4}, new[] {5,6} };
        Console.WriteLine("Flattened: " + string.Join(",", nested.Flatten()));

        Console.WriteLine("\n=== Money extensions ===");
        var price = new Money(9.99m, "USD");
        var tax   = new Money(0.80m, "USD");
        Console.WriteLine(price.Add(tax).Display());          // 10.79 USD
        Console.WriteLine(price.ApplyTax(0.20m).Display());   // 11.99 USD
        Console.WriteLine(price.IsPositive());                 // True
        Console.WriteLine(new Money(0, "USD").IsZero());       // True

        Console.WriteLine("\n=== Interface extensions ===");
        var entity = new AuditedEntity("Order-1", DateTime.UtcNow.AddHours(-3));
        Console.WriteLine(entity.Age());                      // "3h ago"
        Console.WriteLine(entity.IsOlderThan(TimeSpan.FromHours(1)));  // True

        Console.WriteLine("\n=== Nullable extensions ===");
        string? name = "Alice";
        string resolved = name.OrThrow("Name is required");
        Console.WriteLine(resolved);

        string? missing = null;
        try { missing.OrThrow("Name is required"); }
        catch (InvalidOperationException ex) { Console.WriteLine($"Caught: {ex.Message}"); }
    }
}
