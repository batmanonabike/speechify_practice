// ============================================================
// Index and range operators
// ============================================================
//
// Index-from-end uses ^n: ^1 is the last element, ^2 is the
// second-to-last element, and so on.
//
// A range uses start..end. The start is inclusive and the end is
// exclusive. Either side can be omitted, and either side can use
// an index-from-end expression.

namespace CSharpRefresher;

public static class IndexRangeExamples
{
    public static void Run()
    {
        string[] days = ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"];

        // Index operators: ordinary indexes start at zero.
        string first = days[0];
        string last = days[^1];
        string dayBeforeLast = days[^2];
        Console.WriteLine($"First={first}, Last={last}, BeforeLast={dayBeforeLast}");

        // Ranges are start-inclusive and end-exclusive.
        string[] workDays = days[..5];       // Mon through Fri
        string[] weekend = days[5..];        // Sat and Sun
        string[] middle = days[1..^1];       // Tue through Sat
        string[] copy = days[..];            // a shallow array copy

        Console.WriteLine($"Work days: {string.Join(", ", workDays)}");
        Console.WriteLine($"Weekend: {string.Join(", ", weekend)}");
        Console.WriteLine($"Middle: {string.Join(", ", middle)}");
        Console.WriteLine($"Copy length: {copy.Length}");

        // A range on an array creates a new array. Mutating it does not
        // mutate the source array (the elements themselves are shallow-copied).
        weekend[0] = "Saturday";
        Console.WriteLine($"Source still has: {days[5]}");

        // The same syntax works with strings, returning a new string.
        string title = "C# range operators";
        string prefix = title[..2];
        string suffix = title[^9..];
        Console.WriteLine($"Prefix={prefix}, Suffix={suffix}");

        // Use a Range value when the bounds are calculated separately.
        Range firstThree = 0..3;
        Console.WriteLine($"First three: {string.Join(", ", days[firstThree])}");

        // On Span<T>, a range is a view rather than an array allocation.
        Span<int> numbers = [10, 20, 30, 40, 50];
        Span<int> tail = numbers[^2..];
        tail[0] = 99; // changes numbers[3], because the span aliases the source
        Console.WriteLine($"Span source: {string.Join(", ", numbers.ToArray())}");
    }
}
