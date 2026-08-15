// ============================================================
// String manipulation refresher
// ============================================================
// string   — immutable reference type; every "change" is a new allocation.
// StringBuilder — mutable buffer; use for loops or many concatenations.
// Span<char> / Memory<char> — zero-allocation slicing (advanced, .NET 5+).
// ============================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace CSharpRefresher;

public static class StringExamples
{
    public static void Run()
    {
        // ============================================================
        // 1. BASICS — creation, length, indexing
        // ============================================================
        string s = "Hello, World!";
        int    len   = s.Length;          // 13
        char   first = s[0];              // 'H'
        char   last  = s[^1];            // '!'  (index-from-end)
        string sub   = s[7..12];         // "World"  (range operator)

        Console.WriteLine($"Length={len}, first='{first}', last='{last}', sub='{sub}'");

        // ============================================================
        // 2. CASE
        // ============================================================
        string upper = s.ToUpper();                              // "HELLO, WORLD!"
        string lower = s.ToLower();                              // "hello, world!"
        // Culture-safe alternatives for non-ASCII text:
        string upperInv = s.ToUpperInvariant();
        string lowerInv = s.ToLowerInvariant();

        // ============================================================
        // 3. TRIMMING & PADDING
        // ============================================================
        string padded   = "  hello  ";
        string trimmed  = padded.Trim();                         // "hello"
        string trimL    = padded.TrimStart();                    // "hello  "
        string trimR    = padded.TrimEnd();                      // "  hello"
        string trimChar = "***hello***".Trim('*');               // "hello"

        string padRight = "hi".PadRight(10);                     // "hi        "
        string padLeft  = "42".PadLeft(6, '0');                  // "000042"

        Console.WriteLine($"Trimmed: '{trimmed}', PadLeft: '{padLeft}'");

        // ============================================================
        // 4. SEARCHING & TESTING
        // ============================================================
        string text = "The quick brown fox jumps over the lazy dog";

        bool starts  = text.StartsWith("The");                   // true
        bool ends    = text.EndsWith("dog");                     // true
        bool contains= text.Contains("fox");                     // true

        int  idx     = text.IndexOf("fox");                      // 16
        int  lastIdx = text.LastIndexOf("o");                    // 41
        int  idxOf   = text.IndexOf("cat");                      // -1 if not found

        // Case-insensitive search
        bool ciContains = text.Contains("FOX", StringComparison.OrdinalIgnoreCase);

        // Null / empty helpers
        bool isEmpty  = string.IsNullOrEmpty("");                 // true
        bool isBlank  = string.IsNullOrWhiteSpace("   ");        // true

        Console.WriteLine($"IndexOf 'fox': {idx}, contains 'FOX' (ci): {ciContains}");

        // ============================================================
        // 5. SPLITTING & JOINING
        // ============================================================
        string csv   = "alpha,beta,,gamma,delta";
        string[] parts = csv.Split(',');                         // includes empty entry
        string[] noEmpty = csv.Split(',', StringSplitOptions.RemoveEmptyEntries);
        string[] limited = csv.Split(',', 3);                    // max 3 parts: ["alpha","beta","gamma,delta"] -- wait, Split count limits results

        string rejoined = string.Join(" | ", noEmpty);           // "alpha | beta | gamma | delta"
        string joined2  = string.Join(", ", new[] { 1, 2, 3 }); // "1, 2, 3"

        Console.WriteLine($"Split count: {parts.Length}, rejoined: '{rejoined}'");

        // ============================================================
        // 6. REPLACE & REMOVE
        // ============================================================
        string replaced  = text.Replace("fox", "cat");
        string removedSub= text.Remove(startIndex: 0, count: 4); // removes "The "
        string replaced2 = "aabbcc".Replace("bb", "");           // "aacc" — delete by replacing with ""

        // ============================================================
        // 7. INTERPOLATION, FORMAT & COMPOSITE
        // ============================================================
        decimal price = 1234.5m;
        DateTime now  = new DateTime(2026, 8, 14, 9, 30, 0);

        string interp   = $"Price: {price:C2}";                  // culture-aware currency
        string fmt      = string.Format("Price: {0:N2}", price); // "1,234.50"
        string dateFmt  = $"Date: {now:yyyy-MM-dd HH:mm}";       // "Date: 2026-08-14 09:30"
        string inv      = FormattableString.Invariant($"{price:F2}"); // "1234.50" regardless of culture

        Console.WriteLine(interp);
        Console.WriteLine(dateFmt);
        Console.WriteLine($"Invariant: {inv}");

        // ============================================================
        // 8. COMPARE & EQUALITY
        // ============================================================
        string a = "Hello";
        string b = "hello";

        bool refEqual   = ReferenceEquals(a, b);                         // false
        bool valEqual   = a == b;                                         // false
        bool ciEqual    = string.Equals(a, b, StringComparison.OrdinalIgnoreCase); // true
        int  cmp        = string.Compare(a, b, StringComparison.Ordinal);          // < 0

        // Prefer Ordinal(IgnoreCase) for identifiers/keys; CurrentCulture for display text.
        Console.WriteLine($"CI equal: {ciEqual}, Compare: {cmp}");

        // ============================================================
        // 9. SUBSTRING, SLICE, SPAN
        // ============================================================
        string word   = "RefactoringKata";
        string substr = word.Substring(0, 11);                   // "Refactoring" (old API)
        string slice  = word[0..11];                             // "Refactoring" (range, preferred)
        string sliceE = word[11..];                              // "Kata"

        // Span<char> — no heap allocation for intermediate slices
        ReadOnlySpan<char> span = word.AsSpan(0, 11);
        Console.WriteLine($"Span equals: {span.SequenceEqual("Refactoring")}");

        // ============================================================
        // 10. STRINGBUILDER — efficient multi-step construction
        // ============================================================
        var sb = new StringBuilder();
        sb.Append("Hello");
        sb.Append(", ");
        sb.Append("World");
        sb.AppendLine("!");                                       // appends + newline
        sb.AppendFormat("Price: {0:C2}", 9.99m);
        sb.Insert(0, "[START] ");
        sb.Replace("World", "C#");
        string result = sb.ToString();

        // Chaining
        string chained = new StringBuilder()
            .Append("one")
            .Append(", two")
            .Append(", three")
            .ToString();

        Console.WriteLine(result);
        Console.WriteLine(chained);

        // StringBuilder for loop (far cheaper than += in a loop)
        var sbLoop = new StringBuilder(capacity: 256);
        for (int i = 0; i < 5; i++)
            sbLoop.Append(i).Append(' ');
        Console.WriteLine(sbLoop.ToString().TrimEnd());           // "0 1 2 3 4"

        // ============================================================
        // 11. REGEX
        // ============================================================
        string input = "Order #1042 placed on 2026-08-14 for $99.99";

        // IsMatch
        bool hasDate = Regex.IsMatch(input, @"\d{4}-\d{2}-\d{2}");

        // Match — first occurrence
        Match m = Regex.Match(input, @"\d{4}-\d{2}-\d{2}");
        if (m.Success) Console.WriteLine($"Date found: {m.Value}");

        // Named groups
        Match m2 = Regex.Match(input, @"#(?<id>\d+).*\$(?<amount>[\d.]+)");
        if (m2.Success)
            Console.WriteLine($"Order {m2.Groups["id"]}, amount ${m2.Groups["amount"]}");

        // Matches — all occurrences
        MatchCollection digits = Regex.Matches(input, @"\d+");
        Console.WriteLine($"All numbers: {string.Join(", ", digits.Select(d => d.Value))}... wait");
        // Use System.Linq for projection if needed

        // Replace with regex
        string sanitised = Regex.Replace(input, @"\$[\d.]+", "[REDACTED]");
        Console.WriteLine(sanitised);

        // Compiled regex — reuse for hot paths
        var dateRegex = new Regex(@"\d{4}-\d{2}-\d{2}", RegexOptions.Compiled);
        bool found = dateRegex.IsMatch(input);

        // ============================================================
        // 12. PARSING & CONVERSION
        // ============================================================
        int    parsed  = int.Parse("42");
        double parsedD = double.Parse("3.14", CultureInfo.InvariantCulture);
        bool   ok      = int.TryParse("abc", out int safe);      // false, safe=0

        string fromInt = 42.ToString();
        string fromDec = (1234.5m).ToString("N2", CultureInfo.InvariantCulture); // "1,234.50"

        // char operations
        bool isDigit = char.IsDigit('7');
        bool isLetter= char.IsLetter('A');
        bool isUpper = char.IsUpper('A');
        char toLower = char.ToLower('A');

        Console.WriteLine($"Parsed: {parsed}, TryParse ok: {ok}, isDigit: {isDigit}");
    }
}
