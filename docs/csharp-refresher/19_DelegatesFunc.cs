// ============================================================
// Delegates, Func, Action, Predicate
// ============================================================
// A delegate is a type-safe function pointer.
// Func, Action, Predicate are built-in generic delegate types.
//
//   Func<T1,...,TResult>   — takes inputs, returns TResult
//   Action<T1,...>         — takes inputs, returns void
//   Predicate<T>           — takes T, returns bool (== Func<T, bool>)
//
// Lambda expressions create delegate instances inline.
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpRefresher;

public static class DelegatesExamples
{
    // ============================================================
    // 1. Named delegate type (old style — rarely needed today)
    // ============================================================
    public delegate int MathOperation(int a, int b);

    // ============================================================
    // 2. Func — returns a value
    // ============================================================
    private static void FuncDemo()
    {
        Func<int, int, int> add      = (a, b) => a + b;
        Func<string, string> shout   = s => s.ToUpper() + "!";
        Func<int, bool> isEven       = n => n % 2 == 0;
        Func<int> getAnswer          = () => 42;         // no input

        Console.WriteLine(add(3, 4));          // 7
        Console.WriteLine(shout("hello"));     // HELLO!
        Console.WriteLine(isEven(6));          // True
        Console.WriteLine(getAnswer());        // 42

        // Passing Func as a parameter
        Console.WriteLine(Apply(5, x => x * x));          // 25
        Console.WriteLine(Apply(5, x => x + 10));         // 15
    }

    private static T Apply<T>(T value, Func<T, T> transform) => transform(value);

    // ============================================================
    // 3. Action — no return value
    // ============================================================
    private static void ActionDemo()
    {
        Action<string>       print   = msg => Console.WriteLine(msg);
        Action<string, int>  repeat  = (msg, n) => { for (int i = 0; i < n; i++) Console.Write(msg + " "); };
        Action               greet   = () => Console.WriteLine("Hello!");

        print("hi");
        repeat("go", 3);
        Console.WriteLine();
        greet();

        // Action as callback
        ExecuteWithLogging("task1", () => Console.WriteLine("  doing work..."));
    }

    private static void ExecuteWithLogging(string name, Action work)
    {
        Console.WriteLine($"[START] {name}");
        work();
        Console.WriteLine($"[END]   {name}");
    }

    // ============================================================
    // 4. Predicate
    // ============================================================
    private static void PredicateDemo()
    {
        Predicate<int> isPositive = n => n > 0;
        Predicate<string> isLong  = s => s.Length > 5;

        // List.FindAll, List.Find, List.RemoveAll use Predicate<T>
        var numbers = new List<int> { -2, -1, 0, 1, 2, 3 };
        List<int> positives = numbers.FindAll(isPositive);
        Console.WriteLine("Positives: " + string.Join(", ", positives));

        // Predicate is equivalent to Func<T, bool>
        Func<int, bool> isPositiveFunc = n => n > 0;
        // Both work with LINQ:
        var filtered = numbers.Where(isPositiveFunc);
    }

    // ============================================================
    // 5. Multicast delegates — combine multiple methods
    // ============================================================
    private static void MulticastDemo()
    {
        Action<string> log   = msg => Console.WriteLine($"[LOG] {msg}");
        Action<string> audit = msg => Console.WriteLine($"[AUDIT] {msg}");

        Action<string> combined = log + audit;   // invokes both
        combined("user logged in");              // calls log, then audit

        Action<string>? combinedNullable = combined;
        combinedNullable -= log;                           // remove log — may become null
        combinedNullable?.Invoke("user logged out");       // only audit now; ?.Invoke is null-safe
    }

    // ============================================================
    // 6. Closures — capturing outer variables
    // ============================================================
    private static void ClosureDemo()
    {
        int multiplier = 3;
        Func<int, int> triple = x => x * multiplier;   // captures multiplier

        Console.WriteLine(triple(5));   // 15

        multiplier = 10;
        Console.WriteLine(triple(5));   // 50 — captures the VARIABLE, not the value

        // Factory pattern using closures
        Func<int, Func<int, int>> makeAdder = n => x => x + n;
        var addFive = makeAdder(5);
        var addTen  = makeAdder(10);
        Console.WriteLine(addFive(3));  // 8
        Console.WriteLine(addTen(3));   // 13
    }

    // ============================================================
    // 7. Strategy pattern with Func — lightweight alternative to interface
    // ============================================================
    private class PaymentProcessor
    {
        private readonly Func<decimal, decimal> _feeStrategy;

        public PaymentProcessor(Func<decimal, decimal> feeStrategy)
            => _feeStrategy = feeStrategy;

        public decimal CalculateFee(decimal amount) => _feeStrategy(amount);
    }

    private static void StrategyWithFuncDemo()
    {
        var cardProcessor        = new PaymentProcessor(amount => Math.Round(amount * 0.029m + 0.30m, 2));
        var bankTransferProcessor = new PaymentProcessor(amount => Math.Min(Math.Round(amount * 0.01m, 2), 5m));

        Console.WriteLine($"Card fee on $100:          {cardProcessor.CalculateFee(100m):F2}");
        Console.WriteLine($"Bank transfer fee on $100: {bankTransferProcessor.CalculateFee(100m):F2}");
    }

    // ============================================================
    // 8. Func composition and pipelines
    // ============================================================
    private static Func<T, TResult2> Compose<T, TResult1, TResult2>(
        Func<T, TResult1> first,
        Func<TResult1, TResult2> second)
        => x => second(first(x));

    private static void PipelineDemo()
    {
        Func<string, string> trim  = s => s.Trim();
        Func<string, string> upper = s => s.ToUpper();
        Func<string, string> exclaim = s => s + "!";

        var pipeline = Compose(Compose(trim, upper), exclaim);
        Console.WriteLine(pipeline("  hello world  "));   // "HELLO WORLD!"

        // Or use a list of transformations
        var steps = new List<Func<string, string>> { trim, upper, exclaim };
        string result = steps.Aggregate("  hello world  ", (acc, fn) => fn(acc));
        Console.WriteLine(result);
    }

    public static void Run()
    {
        Console.WriteLine("=== Func ===");
        FuncDemo();

        Console.WriteLine("\n=== Action ===");
        ActionDemo();

        Console.WriteLine("\n=== Predicate ===");
        PredicateDemo();

        Console.WriteLine("\n=== Multicast ===");
        MulticastDemo();

        Console.WriteLine("\n=== Closures ===");
        ClosureDemo();

        Console.WriteLine("\n=== Strategy with Func ===");
        StrategyWithFuncDemo();

        Console.WriteLine("\n=== Pipeline ===");
        PipelineDemo();
    }
}
