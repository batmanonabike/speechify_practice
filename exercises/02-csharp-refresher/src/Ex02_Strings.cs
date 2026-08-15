// Exercise 02 - String manipulation
// Reference: docs/csharp-refresher/02_Strings.cs

using System.Text;

namespace CSharpExercises;

public static class StringExercises
{
    /// <summary>
    /// Convert a camelCase or PascalCase identifier to snake_case.
    /// e.g. "MyVariableName" → "my_variable_name"
    /// Hint: iterate chars, insert '_' before each uppercase (not the first).
    /// </summary>
    public static string ToSnakeCase(string input)
    {
        var sb = new StringBuilder();
        foreach (char c in input)
        {
            if (char.IsUpper(c) && sb.Length > 0)
                sb.Append('_');
            sb.Append(char.ToLower(c));
        }

        return sb.ToString();
    }

    /// <summary>
    /// Count how many times <paramref name="sub"/> appears in <paramref name="source"/>
    /// (non-overlapping, case-sensitive).
    /// Hint: IndexOf in a loop advancing by sub.Length each hit.
    /// </summary>
    public static int CountOccurrences(string source, string sub)
    {
        int result = 0;
        int i = source.IndexOf(sub, 0);
        while (i != -1)
        {
            result++;
            i = source.IndexOf(sub, i + sub.Length);
        }

        return result;
    }

    /// <summary>
    /// Truncate <paramref name="text"/> to at most <paramref name="maxLength"/> characters.
    /// If truncated, append <paramref name="ellipsis"/> (default "…").
    /// </summary>
    public static string Truncate(string text, int maxLength, string ellipsis = "…")
    {
        if (text.Length <= maxLength)
            return text;

        int textLength = maxLength - ellipsis.Length;
        if (textLength <= 0)
            return text[..maxLength];

        return text[..textLength] + ellipsis;
    }

    /// <summary>
    /// Reverse the words in a sentence (not the characters within each word).
    /// e.g. "hello world foo" → "foo world hello"
    /// Hint: Split + Reverse + Join.
    /// </summary>
    public static string ReverseWords(string sentence)
    {
        var words = sentence.Split(' ').Reverse();
        return String.Join(' ', words);
    }

    /// <summary>
    /// Return true if <paramref name="s"/> is an anagram of <paramref name="t"/>
    /// (same characters, same frequency, ignoring whitespace and case).
    /// </summary>
    public static bool IsAnagram(string s, string t)
    {
        var normalized1 = s.Where(c => !char.IsWhiteSpace(c))
            .Select(char.ToLower)
            .OrderBy(c => c);

        var normalized2 = t.Where(c => !char.IsWhiteSpace(c))
            .Select(char.ToLower)
            .OrderBy(c => c);

        return normalized1.SequenceEqual(normalized2);
    }

    /// <summary>
    /// Using a StringBuilder, create a comma-separated string of the integers
    /// from 1 to <paramref name="n"/>, with no trailing comma.
    /// e.g. n=5 → "1,2,3,4,5"
    /// </summary>
    public static string CsvOfRange(int n)
    {
        var sb = new StringBuilder();

        int i = 1;
        for (; i < n; ++i)
            sb.Append($"{i},");
        sb.Append($"{i}");
        return sb.ToString();
    }
}
