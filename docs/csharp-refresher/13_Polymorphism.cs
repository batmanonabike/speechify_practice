// ============================================================
// Classes — Polymorphism
// ============================================================
// Polymorphism = "many forms".  In C# this shows up as:
//
//  1. Subtype (runtime) polymorphism — virtual/override dispatch
//  2. Ad-hoc polymorphism           — method overloading
//  3. Parametric polymorphism       — generics  (see separate file)
//
// Key keywords
//   virtual   — opt a base method into override-able dispatch
//   override  — replace a virtual member in a derived class
//   abstract  — force derived classes to provide an implementation
//   new       — HIDE (not override) a base member (usually a smell)
//   sealed    — prevent further overriding of a specific member
// ============================================================

using System;
using System.Collections.Generic;

namespace CSharpRefresher;

// ---- Runtime (subtype) polymorphism ----
public abstract class Shape
{
    public abstract double Area();
    public abstract double Perimeter();

    // Virtual with a default — subclasses may (but need not) override
    public virtual string Describe() =>
        $"{GetType().Name}: area={Area():F2}, perimeter={Perimeter():F2}";
}

public class Circle : Shape
{
    public double Radius { get; }
    public Circle(double radius) => Radius = radius;

    public override double Area()      => Math.PI * Radius * Radius;
    public override double Perimeter() => 2 * Math.PI * Radius;
}

public class Rectangle : Shape
{
    public double Width  { get; }
    public double Height { get; }
    public Rectangle(double width, double height) { Width = width; Height = height; }

    public override double Area()      => Width * Height;
    public override double Perimeter() => 2 * (Width + Height);

    // Also override Describe for a more specific message
    public override string Describe() =>
        base.Describe() + $" [{Width}×{Height}]";
}

public class Square : Rectangle
{
    public Square(double side) : base(side, side) { }

    // sealed — Triangle cannot override this if it derived from Square
    public sealed override string Describe() =>
        $"Square: side={Width}, area={Area():F2}";
}

// ---- Hiding with `new` (NOT polymorphic — usually a design smell) ----
public class SpecialShape : Shape
{
    public override double Area()      => 0;
    public override double Perimeter() => 0;

    // new hides the base virtual Describe — only visible when ref is SpecialShape
    public new string Describe() => "I am special (hidden, not overridden)";
}

// ---- Method hiding vs overriding demo ----
public static class HidingVsOverriding
{
    public static void Demo()
    {
        var special = new SpecialShape();
        Shape asBase = special;

        Console.WriteLine(special.Describe());   // "I am special..."  — uses SpecialShape.Describe (new)
        Console.WriteLine(asBase.Describe());    // "SpecialShape: area=0..." — uses Shape.Describe (virtual)
    }
}

// ---- Pattern matching — a modern polymorphism tool ----
public static class PatternDispatch
{
    public static string Classify(Shape shape) => shape switch
    {
        Circle  c when c.Radius > 10 => $"Large circle (r={c.Radius})",
        Circle  c                    => $"Small circle (r={c.Radius})",
        Square  s                    => $"Square (side={s.Width})",
        Rectangle r                  => $"Rectangle ({r.Width}×{r.Height})",
        _                            => $"Unknown shape: {shape.GetType().Name}"
    };
}

public static class PolymorphismExamples
{
    public static void Run()
    {
        // ---- Subtype polymorphism via base-class list ----
        var shapes = new List<Shape>
        {
            new Circle(5),
            new Circle(15),
            new Rectangle(4, 6),
            new Square(3),
        };

        Console.WriteLine("== Virtual dispatch ==");
        foreach (var s in shapes)
            Console.WriteLine("  " + s.Describe());   // correct override called at runtime

        // ---- Total area — polymorphic without knowing concrete types ----
        double totalArea = 0;
        foreach (var s in shapes) totalArea += s.Area();
        Console.WriteLine($"Total area: {totalArea:F2}");

        // ---- Pattern matching dispatch ----
        Console.WriteLine("\n== Pattern matching ==");
        foreach (var s in shapes)
            Console.WriteLine("  " + PatternDispatch.Classify(s));

        // ---- Hiding vs overriding ----
        Console.WriteLine("\n== Hiding vs overriding ==");
        HidingVsOverriding.Demo();

        // ---- is / as ----
        Shape maybeCircle = new Circle(7);
        if (maybeCircle is Circle c)
            Console.WriteLine($"\nCast succeeded, radius = {c.Radius}");

        Rectangle? maybeRect = maybeCircle as Rectangle;   // null — safe cast
        Console.WriteLine($"As Rectangle: {maybeRect?.ToString() ?? "null"}");
    }
}
