// Exercise 01 - Arrays, const, type aliases
// Reference: docs/csharp-refresher/01_ArraysConstAliases.cs

namespace CSharpExercises;

using KeyAn = (int key, int distinctCount);

public static class ArrayExercises
{
    /// <summary>
    /// Return the second-largest DISTINCT value in <paramref name="arr"/>.
    /// Throw InvalidOperationException if fewer than 2 distinct values exist.
    /// Hint: sort descending and skip first — or use a set.
    /// </summary>
    public static int SecondLargest(int[] arr)
    {
        var d = arr.Distinct().OrderByDescending(i => i).ToArray();
        if (d.Length < 2)
            throw new InvalidOperationException();
        return d[1];
    }

    /// <summary>
    /// Rotate a 2D matrix (square, n×n) 90 degrees clockwise IN PLACE.
    /// Classic algorithm: transpose then reverse each row.
    /// </summary>
    public static void RotateMatrix90(int[,] matrix)
        => throw new NotImplementedException();

    /// <summary>
    /// Use ReadOnlySpan<char> to count how many times <paramref name="c"/>
    /// appears in <paramref name="text"/> without allocating a new string.
    /// </summary>
    public static int CountChar(ReadOnlySpan<char> text, char c)
        => text.Count(c);

    // ---------------------------------------------------------------
    // Constants — define the values below as const or static readonly
    // depending on which is appropriate.
    // ---------------------------------------------------------------

    /// <summary>
    /// Maximum number of items per page in the UI.
    /// Appropriate modifier: const int (compile-time constant).
    /// </summary>
    public const int PageSize = 25; // TODO: replace 0 with the real value (e.g. 25)

    /// <summary>
    /// Default timeout expressed as a TimeSpan.
    /// Appropriate modifier: static readonly (not a compile-time constant).
    /// </summary>
    public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30); // TODO: e.g. 30 seconds
}
