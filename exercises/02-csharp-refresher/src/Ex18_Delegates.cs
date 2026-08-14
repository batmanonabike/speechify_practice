// Exercise 18 — Delegates, Func, Action, Predicate, events
// Reference: docs/csharp-refresher/18_Delegates.cs

namespace CSharpExercises;

public static class DelegateExercises
{
    /// <summary>
    /// Apply <paramref name="transform"/> to every element of <paramref name="source"/>
    /// and return the results.  (Your own mini Select.)
    /// </summary>
    public static IEnumerable<TResult> Map<T, TResult>(
        IEnumerable<T> source, Func<T, TResult> transform)
        => throw new NotImplementedException();

    /// <summary>
    /// Compose two Func delegates: return a new Func that first applies
    /// <paramref name="first"/> then <paramref name="second"/>.
    /// Hint: x => second(first(x))
    /// </summary>
    public static Func<T, TResult> Compose<T, TMiddle, TResult>(
        Func<T, TMiddle> first,
        Func<TMiddle, TResult> second)
        => throw new NotImplementedException();

    /// <summary>
    /// Run <paramref name="action"/> <paramref name="times"/> times,
    /// passing the current iteration index (0-based).
    /// </summary>
    public static void Repeat(int times, Action<int> action)
        => throw new NotImplementedException();

    /// <summary>
    /// Return a cached (memoized) version of <paramref name="fn"/>.
    /// The same input should only be computed once; subsequent calls use the cache.
    /// Hint: Dictionary + closure.
    /// </summary>
    public static Func<TArg, TResult> Memoize<TArg, TResult>(Func<TArg, TResult> fn)
        where TArg : notnull
        => throw new NotImplementedException();
}

// ---------------------------------------------------------------
// Event practice
// ---------------------------------------------------------------

public class StockTicker
{
    /// <summary>
    /// Raised whenever the price changes.
    /// EventArgs carries the new price (double).
    /// </summary>
    public event EventHandler<double>? PriceChanged;

    private double _price;

    /// <summary>
    /// Setting this property should raise PriceChanged only when the value
    /// actually differs from the previous price.
    /// </summary>
    public double Price
    {
        get => _price;
        set => throw new NotImplementedException();
    }
}
