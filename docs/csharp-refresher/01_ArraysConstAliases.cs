// ============================================================
// Arrays, const, readonly, and using aliases
// ============================================================

using System;
using System.Collections.Generic;

// ============================================================
// USING ALIASES
// ============================================================
// Rename a type (or namespace) for this file.
// Useful for:
//   - Avoiding ambiguity between two types with the same name
//   - Shortening verbose generic types
//   - Documenting intent at the call site

using Point2D  = (double X, double Y);                         // tuple alias (C# 12)
using IntGrid  = System.Collections.Generic.List<int[]>;
using Lookup   = System.Collections.Generic.Dictionary<string, int>;
using Vec3     = (double X, double Y, double Z);

// Disambiguate when two namespaces expose a type called "Timer"
using ThreadingTimer  = System.Threading.Timer;
// using TimersTimer  = System.Timers.Timer;   // would clash without alias

namespace CSharpRefresher;

public static class ArraysConstAliasExamples
{
    // ============================================================
    // CONST
    // ============================================================
    // - Evaluated and inlined at compile time
    // - Must be a primitive, string, or enum value
    // - Implicitly static — no instance required
    // - Cannot be changed at runtime
    public const double Pi          = 3.14159265358979;
    public const string AppName     = "CSharpRefresher";
    public const int    MaxRetries  = 3;
    public const bool   DebugMode   = false;

    // ============================================================
    // READONLY FIELDS
    // ============================================================
    // - Set once: at declaration OR in the constructor
    // - Can be any type (including reference types)
    // - Instance or static
    // - Value is not inlined — evaluated at runtime
    // Instance readonly field — must live in a non-static class
    private class Widget
    {
        private readonly int _instanceId;                          // set in constructor only
        private static readonly Random _rng = new();              // shared, set once
        public Widget(int id) => _instanceId = id;
        public override string ToString() => $"Widget#{_instanceId}";
    }

    // ============================================================
    // const vs readonly — KEY DIFFERENCES
    // ============================================================
    //
    //                  const           readonly field
    // Evaluated        compile time    runtime
    // Allowed types    primitives      any type
    //                  string, enum
    // Static           always          instance or static
    // In interface     no              no (fields not in interfaces)
    // Versioning       INLINED — if    safe across assemblies
    //                  value changes
    //                  callers need
    //                  recompile

    // ============================================================
    // READONLY STRUCTS & MEMBERS
    // ============================================================
    // readonly struct  — entire struct is immutable; all members readonly
    // readonly method  — promises not to mutate the struct instance

    public readonly struct Celsius(double value)
    {
        public double Value     { get; } = value;
        public double ToFahrenheit() => Value * 9 / 5 + 32;
        public override string ToString() => $"{Value}°C";
    }

    // ============================================================
    // ARRAYS
    // ============================================================

    public static void Run()
    {
        // ---- Declaration and initialisation ----
        int[]    zeros    = new int[5];                        // [0,0,0,0,0] — default-initialised
        int[]    primes   = new int[] { 2, 3, 5, 7, 11 };     // explicit new
        int[]    primes2  = { 2, 3, 5, 7, 11 };               // array initialiser shorthand
        int[]    primes3  = [2, 3, 5, 7, 11];                 // collection expression (C# 12)
        string[] names    = ["Alice", "Bob", "Carol"];

        // ---- Access ----
        int first = primes[0];          // 2
        int last  = primes[^1];         // 11  (index-from-end)
        int[] sub = primes[1..4];       // [3, 5, 7]  (range — creates new array)

        Console.WriteLine($"First={first}, Last={last}, Sub=[{string.Join(",",sub)}]");

        // ---- Length and bounds ----
        int len = primes.Length;        // 5
        // primes[5] would throw IndexOutOfRangeException

        // ---- Iteration ----
        foreach (int prime in primes)
            Console.Write(prime + " ");
        Console.WriteLine();

        for (int i = 0; i < primes.Length; i++)
            Console.Write($"[{i}]={primes[i]} ");
        Console.WriteLine();

        // ---- Sorting and searching ----
        int[] data = [5, 3, 1, 4, 2];
        Array.Sort(data);                          // in-place sort
        int idx = Array.BinarySearch(data, 3);     // requires sorted array → index 2
        Array.Reverse(data);                       // in-place reverse
        Console.WriteLine($"Sorted+reversed: [{string.Join(",", data)}]");

        // ---- Copy ----
        int[] copy1 = (int[])primes.Clone();       // shallow copy
        int[] copy2 = new int[primes.Length];
        Array.Copy(primes, copy2, primes.Length);  // copy into existing array

        int[] copy3 = primes[..];                  // range copy — full slice

        // ---- Array utility methods ----
        bool exists = Array.Exists(primes, p => p > 10);       // true
        int  found  = Array.Find(primes, p => p > 4);          // 5
        int  foundI = Array.FindIndex(primes, p => p > 4);     // index 2
        int[] all   = Array.FindAll(primes, p => p % 2 != 0);  // [3,5,7,11]

        Array.Fill(zeros, 42);                                  // [42,42,42,42,42]
        Console.WriteLine($"Filled: [{string.Join(",", zeros)}]");

        // ---- 2D arrays ----
        int[,] matrix = new int[3, 3];
        matrix[0, 0] = 1;
        matrix[1, 1] = 5;
        matrix[2, 2] = 9;

        // Rectangular 2D with initialiser
        int[,] grid = { { 1, 2, 3 },
                         { 4, 5, 6 },
                         { 7, 8, 9 } };

        int rows = grid.GetLength(0);   // 3
        int cols = grid.GetLength(1);   // 3

        for (int r = 0; r < rows; r++)
        {
            for (int col = 0; col < cols; col++)
                Console.Write($"{grid[r, col]} ");
            Console.WriteLine();
        }

        // ---- Jagged arrays (array of arrays — rows can have different lengths) ----
        int[][] jagged = new int[3][];
        jagged[0] = [1];
        jagged[1] = [2, 3];
        jagged[2] = [4, 5, 6];

        foreach (var row in jagged)
            Console.WriteLine("[" + string.Join(",", row) + "]");

        // ---- Span<T> — zero-allocation slice over array (or stack memory) ----
        Span<int> span       = primes.AsSpan();          // entire array
        Span<int> spanSlice  = primes.AsSpan(1, 3);      // elements 1,2,3 — no copy
        spanSlice[0] = 99;                               // mutates primes[1] directly!

        ReadOnlySpan<int> roSpan = primes;               // read-only view

        // Stack-allocated array via stackalloc (no GC pressure)
        Span<int> stackArr = stackalloc int[8];
        for (int i = 0; i < stackArr.Length; i++) stackArr[i] = i * i;
        Console.WriteLine($"Stack squares: [{string.Join(",", stackArr.ToArray())}]");

        // ---- Array covariance (reference types only — be careful) ----
        string[] strings = ["hello", "world"];
        object[] objects = strings;                      // valid — array covariance
        // objects[0] = 42;                             // throws ArrayTypeMismatchException at runtime!

        // ---- const demo ----
        Console.WriteLine($"\nconst Pi={Pi}, MaxRetries={MaxRetries}, App={AppName}");

        // ---- readonly demo ----
        var cel = new Celsius(100);
        Console.WriteLine($"{cel} = {cel.ToFahrenheit()}°F");
        var w = new Widget(42);
        Console.WriteLine($"Widget readonly instance: {w}");

        // static readonly field (defined on Widget)
        IReadOnlyList<string> supportedCurrencies = ["USD", "EUR", "GBP", "CAD"];
        Console.WriteLine($"Supported currencies: {string.Join(", ", supportedCurrencies)}");

        // ---- using aliases demo ----
        Point2D pt = (3.0, 4.0);
        Console.WriteLine($"\nPoint2D alias: ({pt.X}, {pt.Y})");

        Lookup scores = new() { ["Alice"] = 95, ["Bob"] = 82 };
        Console.WriteLine($"Lookup alias: Alice={scores["Alice"]}");

        Vec3 v = (1.0, 2.0, 3.0);
        Console.WriteLine($"Vec3 alias: ({v.X}, {v.Y}, {v.Z})");

        IntGrid intGrid = [[1, 2, 3], [4, 5, 6]];
        Console.WriteLine($"IntGrid alias rows: {intGrid.Count}");
    }
}
