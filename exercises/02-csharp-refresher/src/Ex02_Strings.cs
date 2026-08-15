// Exercise 02 - String manipulation
// Reference: docs/csharp-refresher/02_Strings.cs

namespace CSharpExercises;

public static class StringExercises
{
    /// <summary>
    /// Convert a camelCase or PascalCase identifier to snake_case.
    /// e.g. "MyVariableName" → "my_variable_name"
    /// Hint: iterate chars, insert '_' before each uppercase (not the first).
    /// </summary>
    public static string ToSnakeCase(string input)
        => throw new NotImplementedException();

    /// <summary>
    /// Count how many times <paramref name="sub"/> appears in <paramref name="source"/>
    /// (non-overlapping, case-sensitive).
    /// Hint: IndexOf in a loop advancing by sub.Length each hit.
    /// </summary>
    public static int CountOccurrences(string source, string sub)
        => throw new NotImplementedException();

    /// <summary>
    /// Truncate <paramref name="text"/> to at most <paramref name="maxLength"/> characters.
    /// If truncated, append <paramref name="ellipsis"/> (default "…").
    /// </summary>
    public static string Truncate(string text, int maxLength, string ellipsis = "…")
        => throw new NotImplementedException();

    /// <summary>
    /// Reverse the words in a sentence (not the characters within each word).
    /// e.g. "hello world foo" → "foo world hello"
    /// Hint: Split + Reverse + Join.
    /// </summary>
    public static string ReverseWords(string sentence)
        => throw new NotImplementedException();

    /// <summary>
    /// Return true if <paramref name="s"/> is an anagram of <paramref name="t"/>
    /// (same characters, same frequency, ignoring whitespace and case).
    /// </summary>
    public static bool IsAnagram(string s, string t)
        => throw new NotImplementedException();

    /// <summary>
    /// Using a StringBuilder, create a comma-separated string of the integers
    /// from 1 to <paramref name="n"/>, with no trailing comma.
    /// e.g. n=5 → "1,2,3,4,5"
    /// </summary>
    public static string CsvOfRange(int n)
        => throw new NotImplementedException();
}
