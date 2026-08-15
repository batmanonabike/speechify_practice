// ============================================================
// Classes — Interface inheritance
// ============================================================
// Interfaces define a contract (what, not how).
// A class can implement multiple interfaces.
// Interfaces can:
//   - have default implementations (C# 8+)
//   - inherit from other interfaces
//   - define static abstract members (C# 11+)
// ============================================================

using System;
using System.Collections.Generic;

namespace CSharpRefresher;

// ---- Simple interfaces ----
public interface IDescribable
{
    string Describe();
}

public interface IPriceable
{
    decimal Price { get; }
    decimal PriceWithTax(decimal taxRate);
}

// Interface inheriting from another interface
public interface IDiscountable : IPriceable
{
    decimal Discount { get; }
    decimal DiscountedPrice => Price - Discount;    // default implementation (C# 8+)
}

// ---- Implementing multiple interfaces ----
public class Product : IDescribable, IDiscountable
{
    public string  Name     { get; }
    public decimal Price    { get; }
    public decimal Discount { get; }

    public Product(string name, decimal price, decimal discount = 0m)
    {
        Name     = name;
        Price    = price;
        Discount = discount;
    }

    public string  Describe()                      => $"{Name} (${Price:F2})";
    public decimal PriceWithTax(decimal taxRate)   => Price * (1 + taxRate);

    // DiscountedPrice is NOT overridden — the default implementation on IDiscountable is used
}

// ---- Explicit interface implementation ----
// Useful when two interfaces have a member with the same name but different semantics.
public interface IMetric  { string Format(); }
public interface IImperial { string Format(); }

public class Distance : IMetric, IImperial
{
    private readonly double _metres;
    public Distance(double metres) => _metres = metres;

    // Explicit — only accessible through the interface reference
    string IMetric.Format()   => $"{_metres:F2} m";
    string IImperial.Format() => $"{_metres * 3.281:F2} ft";
}

// ---- Interface as abstraction boundary (dependency inversion) ----
public interface ILogger
{
    void Log(string message);
}

public interface IRepository<T>
{
    void   Save(T item);
    T?     FindById(int id);
    IEnumerable<T> GetAll();
}

public class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine($"[LOG] {message}");
}

// Generic class implementing a generic interface
public class InMemoryRepository<T> : IRepository<T>
{
    private readonly Dictionary<int, T> _store = [];
    private int _nextId = 1;

    public void Save(T item)
    {
        _store[_nextId++] = item;
    }

    public T? FindById(int id) =>
        _store.TryGetValue(id, out var item) ? item : default;

    public IEnumerable<T> GetAll() => _store.Values;
}

public static class InterfaceExamples
{
    public static void Run()
    {
        // ---- Multiple interface satisfaction ----
        var p = new Product("Widget", 9.99m, discount: 1.00m);
        Console.WriteLine(p.Describe());                      // Widget ($9.99)
        Console.WriteLine($"With 20% tax: {p.PriceWithTax(0.20m):F2}");
        Console.WriteLine($"Discounted:   {((IDiscountable)p).DiscountedPrice:F2}");  // default interface member — must access via interface ref

        // Upcast to interface — only interface members visible
        IPriceable priceable = p;
        Console.WriteLine($"Price via interface: {priceable.Price}");

        // ---- Explicit interface implementation ----
        var dist = new Distance(1.5);
        Console.WriteLine(((IMetric)dist).Format());     // 1.50 m
        Console.WriteLine(((IImperial)dist).Format());   // 4.92 ft

        // ---- Interface in generic / polymorphic context ----
        ILogger logger = new ConsoleLogger();
        logger.Log("Application started");

        var repo = new InMemoryRepository<Product>();
        repo.Save(new Product("Alpha", 5m));
        repo.Save(new Product("Beta",  8m));

        foreach (var item in repo.GetAll())
            Console.WriteLine(item.Describe());

        // ---- is / as with interfaces ----
        IDescribable describable = p;
        if (describable is IDiscountable disc)
            Console.WriteLine($"Also discountable: {disc.DiscountedPrice:F2}");
    }
}
