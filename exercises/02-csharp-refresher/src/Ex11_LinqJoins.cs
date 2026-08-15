// Exercise 11 - LINQ: Joins & Zip
// Reference: docs/csharp-refresher/11_Linq_JoinsZipSets.cs

namespace CSharpExercises;

public static class LinqJoinExercises
{
    /// <summary>
    /// Given a sequence, return a sequence of (item, index) tuples.
    /// Hint: Select with index overload, or Zip with Enumerable.Range.
    /// </summary>
    public static IEnumerable<(T Item, int Index)> WithIndex<T>(IEnumerable<T> source)
        => throw new NotImplementedException();

    /// <summary>
    /// Left-join <paramref name="lines"/> to <paramref name="discounts"/> on Product name.
    /// Return each line paired with its discount rate (0 if not found).
    /// Hint: GroupJoin + SelectMany with DefaultIfEmpty.
    /// </summary>
    public static IEnumerable<(OrderLine Line, decimal DiscountRate)> ApplyDiscounts(
        IEnumerable<OrderLine> lines,
        IEnumerable<(string Product, decimal Rate)> discounts)
        => throw new NotImplementedException();

    /// <summary>
    /// Inner-join two sequences on a shared key, returning paired results.
    /// Hint: Join.
    /// </summary>
    public static IEnumerable<(TLeft Left, TRight Right)> InnerJoin<TLeft, TRight, TKey>(
        IEnumerable<TLeft>  left,
        IEnumerable<TRight> right,
        Func<TLeft,  TKey>  leftKey,
        Func<TRight, TKey>  rightKey)
        => throw new NotImplementedException();
}
