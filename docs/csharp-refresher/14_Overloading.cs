// ============================================================
// Classes — Method overloading
// ============================================================
// Overloading = same method name, different parameter signatures.
// The compiler picks the best match at compile time (static dispatch).
// Not the same as overriding (which is runtime dispatch).
//
// Rules:
//   - Must differ in number OR types of parameters
//   - Return type alone is NOT enough to distinguish overloads
//   - Optional parameters and params can cause ambiguity — be careful
// ============================================================

using System;

namespace CSharpRefresher;

public static class OverloadExamples
{
    // ---- Basic overloading — same name, different parameter types ----
    public static void Print(int value)     => Console.WriteLine($"int:     {value}");
    public static void Print(double value)  => Console.WriteLine($"double:  {value}");
    public static void Print(string value)  => Console.WriteLine($"string:  {value}");
    public static void Print(bool value)    => Console.WriteLine($"bool:    {value}");

    // ---- Different number of parameters ----
    public static int Add(int a, int b)           => a + b;
    public static int Add(int a, int b, int c)    => a + b + c;
    public static double Add(double a, double b)  => a + b;

    // ---- Optional parameters — use carefully to avoid ambiguity ----
    // Add(1, 2) is ambiguous between Add(int,int) and Add(int,int,int=0) if both exist.
    // Safe here because the int overload requires exactly 2 args.
    public static string Format(string text, bool uppercase = false) =>
        uppercase ? text.ToUpper() : text;

    public static string Format(string text, int repeatCount) =>
        string.Concat(Enumerable.Repeat(text, repeatCount));

    // ---- params overload ----
    public static int Sum(params int[] values) => values.Length == 0 ? 0 : values.Sum();

    // ---- Operator overloading ----
    // Operators are special static methods named `operator <op>`.
    public readonly struct Vector2D(double x, double y)
    {
        public double X { get; } = x;
        public double Y { get; } = y;

        public static Vector2D operator +(Vector2D a, Vector2D b) => new(a.X + b.X, a.Y + b.Y);
        public static Vector2D operator -(Vector2D a, Vector2D b) => new(a.X - b.X, a.Y - b.Y);
        public static Vector2D operator *(Vector2D v, double scalar) => new(v.X * scalar, v.Y * scalar);
        public static Vector2D operator *(double scalar, Vector2D v) => v * scalar;  // commutative
        public static bool     operator ==(Vector2D a, Vector2D b) => a.X == b.X && a.Y == b.Y;
        public static bool     operator !=(Vector2D a, Vector2D b) => !(a == b);

        // When overloading == you should also override Equals and GetHashCode
        public override bool Equals(object? obj) => obj is Vector2D v && this == v;
        public override int  GetHashCode()       => HashCode.Combine(X, Y);
        public override string ToString()        => $"({X}, {Y})";

        public double Length => Math.Sqrt(X * X + Y * Y);
    }

    public static void Run()
    {
        // ---- Method overloads ----
        Print(42);
        Print(3.14);
        Print("hello");
        Print(true);

        Console.WriteLine(Add(1, 2));        // 3  — int overload
        Console.WriteLine(Add(1, 2, 3));     // 6  — three-arg overload
        Console.WriteLine(Add(1.5, 2.5));    // 4  — double overload

        Console.WriteLine(Format("hi"));                // "hi"
        Console.WriteLine(Format("hi", uppercase: true)); // "HI"
        Console.WriteLine(Format("ab", 3));             // "ababab"

        Console.WriteLine(Sum());            // 0
        Console.WriteLine(Sum(1, 2, 3, 4)); // 10

        // ---- Operator overloads ----
        var v1 = new Vector2D(1, 2);
        var v2 = new Vector2D(3, 4);

        Console.WriteLine(v1 + v2);          // (4, 6)
        Console.WriteLine(v2 - v1);          // (2, 2)
        Console.WriteLine(v1 * 3);           // (3, 6)
        Console.WriteLine(2 * v2);           // (6, 8)
        Console.WriteLine(v1 == new Vector2D(1, 2));  // True
        Console.WriteLine($"|v2| = {v2.Length:F4}");  // 5.0000
    }
}
