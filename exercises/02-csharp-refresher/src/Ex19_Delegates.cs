// Exercise 19 - Delegates, Func, Action, Predicate, events
// Reference: docs/csharp-refresher/19_DelegatesFunc.cs

namespace CSharpExercises;

public static class DelegateExercises
{
    /// <summary>
    /// Apply <paramref name="transform"/> to every element of <paramref name="source"/>
    /// and return the results.  (Your own mini Select.)
    /// </summary>
    public static IEnumerable<TResult> Map<T, TResult>(
        IEnumerable<T> source, Func<T, TResult> transform)
    {
        foreach (var item in source)
            yield return transform(item);
    }

    /// <summary>
    /// Compose two Func delegates: return a new Func that first applies
    /// <paramref name="first"/> then <paramref name="second"/>.
    /// Hint: x => second(first(x))
    /// </summary>
    public static Func<T, TResult> Compose<T, TMiddle, TResult>(
        Func<T, TMiddle> first,
        Func<TMiddle, TResult> second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);
        return a => second(first(a));
    }

    /// <summary>
    /// Run <paramref name="action"/> <paramref name="times"/> times,
    /// passing the current iteration index (0-based).
    /// </summary>
    public static void Repeat(int times, Action<int> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        for (int n = 0; n < times; n++)
            action(n);
    }

    /// <summary>
    /// Return a cached (memoized) version of <paramref name="fn"/>.
    /// The same input should only be computed once; subsequent calls use the cache.
    /// Hint: Dictionary + closure.
    /// </summary>
    public static Func<TArg, TResult> Memoize<TArg, TResult>(Func<TArg, TResult> fn)
        where TArg : notnull
    {
        ArgumentNullException.ThrowIfNull(fn);

        var cache = new Dictionary<TArg, TResult>();

        return (arg) =>
        {
            if (cache.TryGetValue(arg, out var result))
                return result;

            result = fn(arg);
            cache[arg] = result;
            return result;
        };
    }
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
        set
        {
            if (_price != value)
            {
                _price = value;
                PriceChanged?.Invoke(this, value);
            }
        }

    }
}
