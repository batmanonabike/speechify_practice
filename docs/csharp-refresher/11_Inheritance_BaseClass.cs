// ============================================================
// Classes — Inheritance: base classes & constructors
// ============================================================
// A derived class extends a base class with `: BaseClass`.
// Use `base(...)` to explicitly invoke the base constructor.
// Mark base members `virtual` to allow overriding.
// Mark derived overrides with `override`.
// Seal a class or method with `sealed` to prevent further derivation.
// ============================================================

using System;

namespace CSharpRefresher;

// ---- Base class ----
public abstract class Animal
{
    // Properties set in constructor (see Properties file for full get/set coverage)
    public string Name  { get; }
    public int    Age   { get; }

    // Base constructor — derived classes must supply Name and Age
    protected Animal(string name, int age)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
        Age  = age;
    }

    // Virtual method — derived classes may override
    public virtual string Speak() => $"{Name} makes a sound.";

    // Non-virtual method — cannot be overridden (only hidden with `new`)
    public string Describe() => $"{Name}, age {Age}";

    // Abstract method — derived classes MUST override
    public abstract string Diet();

    public override string ToString() => $"[{GetType().Name}] {Describe()}";
}

// ---- Derived class — Dog ----
public class Dog : Animal
{
    public string Breed { get; }

    // Invoke base constructor with base(name, age)
    public Dog(string name, int age, string breed) : base(name, age)
    {
        Breed = breed;
    }

    public override string Speak() => $"{Name} barks!";
    public override string Diet()  => "Omnivore";
}

// ---- Derived class — Cat ----
public class Cat : Animal
{
    public bool IsIndoor { get; }

    public Cat(string name, int age, bool isIndoor) : base(name, age)
    {
        IsIndoor = isIndoor;
    }

    public override string Speak() => $"{Name} meows.";
    public override string Diet()  => "Carnivore";
}

// ---- Further derived — sealed prevents further subclassing ----
public sealed class GuideDog : Dog
{
    public string Handler { get; }

    public GuideDog(string name, int age, string breed, string handler)
        : base(name, age, breed)   // calls Dog's constructor, which calls Animal's
    {
        Handler = handler;
    }

    // sealed override — no class deriving from GuideDog (impossible anyway) can override
    public sealed override string Speak() => $"{Name} whimpers quietly.";
}

// ---- Calling base method explicitly ----
public class VerboseDog : Dog
{
    public VerboseDog(string name, int age) : base(name, age, "Mixed") { }

    public override string Speak()
    {
        string baseSound = base.Speak();   // explicitly call Dog.Speak()
        return $"{baseSound} (very loudly!)";
    }
}

public static class InheritanceExamples
{
    public static void Run()
    {
        var dog      = new Dog("Rex", 3, "Labrador");
        var cat      = new Cat("Whiskers", 5, isIndoor: true);
        var guide    = new GuideDog("Buddy", 4, "Golden Retriever", "Sarah");
        var verbose  = new VerboseDog("Max", 2);

        Console.WriteLine(dog.Speak());           // Rex barks!
        Console.WriteLine(cat.Speak());           // Whiskers meows.
        Console.WriteLine(guide.Speak());         // Buddy whimpers quietly.
        Console.WriteLine(verbose.Speak());       // Max barks! (very loudly!)

        Console.WriteLine(dog.Diet());            // Omnivore
        Console.WriteLine(dog.Describe());        // Rex, age 3  (non-virtual, same for all)

        // Base type reference — polymorphic dispatch
        Animal a = dog;
        Console.WriteLine(a.Speak());             // still calls Dog.Speak() — virtual dispatch

        // is / as / pattern matching
        if (guide is Dog d)
            Console.WriteLine($"GuideDog is a Dog, breed: {d.Breed}");

        Animal[] animals = [dog, cat, guide, verbose];
        foreach (var animal in animals)
            Console.WriteLine(animal);            // calls overridden ToString on Animal
    }
}
