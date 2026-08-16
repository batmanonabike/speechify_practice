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
    {
        return source.Select((item, i) => (item, i));
        // return source.Select((item, i) => (Item: item, Index: i));
    }

    /// <summary>
    /// Left-join <paramref name="lines"/> to <paramref name="discounts"/> on Product name.
    /// Return each line paired with its discount rate (0 if not found).
    /// Hint: GroupJoin + SelectMany with DefaultIfEmpty.
    /// </summary>
    public static IEnumerable<(OrderLine Line, decimal DiscountRate)> ApplyDiscounts(
        IEnumerable<OrderLine> lines,
        IEnumerable<(string Product, decimal Rate)> discounts)
    {
        // SELECT
        //     lines.Product,
        //     lines.Category,
        //     lines.Price,
        //     lines.Quantity,
        //     COALESCE(discounts.Rate, 0) AS Rate
        // FROM lines
        // LEFT OUTER JOIN
        //     discounts ON lines.Product = discounts.Product

        //var x = lines.GroupJoin(
        //    discounts,
        //    leftKey => leftKey.Product, // lines
        //    rightKey => rightKey.Product, // discounts
        //    (line, matches) => new
        //    {
        //        TheLine = line,
        //        Matches = matches // IEnumerable<(string Product, decimal Rate)> maybe empty.
        //    });

        var x = lines.GroupJoin(
            discounts,
            leftKey => leftKey.Product, // lines
            rightKey => rightKey.Product, // discounts
            (line, matches) => (TheLine: line, Matches: matches));

        var y = x.SelectMany(
            itemPair => itemPair.Matches.DefaultIfEmpty(), // Func to flatten the matches (the right hand part of the join)
            (lhs, rhs) => (lhs.TheLine, rhs.Rate) // Build the result tuple.  rhs is the reduced matches.
            );

        return y;
    }

    /// <summary>
    /// Inner-join two sequences on a shared key, returning paired results.
    /// Hint: Join.
    /// </summary>
    public static IEnumerable<(TLeft Left, TRight Right)> InnerJoin<TLeft, TRight, TKey>(
        IEnumerable<TLeft> left,
        IEnumerable<TRight> right,
        Func<TLeft, TKey> leftKey,
        Func<TRight, TKey> rightKey)
    {
        return left.Join(
            right,
            l => leftKey(l),
            r => rightKey(r),
            (lhs, rhs) => (lhs, rhs));
    }
}