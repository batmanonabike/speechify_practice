// Exercise 22 - Records vs Classes
// Reference: docs/csharp-refresher/22_RecordsVsClasses.cs

namespace CSharpExercises;

// ---------------------------------------------------------------
// Your task: define the types and implement the helpers below.
// ---------------------------------------------------------------

/// <summary>
/// Define a RECORD called ProductRecord with properties:
///   string Id, string Name, decimal Price
/// Records give you value equality and With-expressions for free.
/// </summary>
public record ProductRecord(string Id, string Name, decimal Price);

/// <summary>
/// Define a CLASS called ProductClass with the same three properties.
/// Override Equals and GetHashCode so two instances with the same Id
/// are considered equal (Id is the identity key).
/// </summary>
public class ProductClass
{
    public string Id { get; }
    public string Name { get; }
    public decimal Price { get; }

    public ProductClass(string id, string name, decimal price)
    { Id = id; Name = name; Price = price; }

    // TODO: override Equals (by Id) and GetHashCode
    public override bool Equals(object? obj) 
        => obj is ProductClass other && string.Equals(Id, other.Id, StringComparison.Ordinal);
    public override int GetHashCode() => Id.GetHashCode();
}

public static class RecordExercises
{
    /// <summary>
    /// Given a ProductRecord, return a new one with the Price increased by
    /// <paramref name="amount"/> using a with-expression.
    /// </summary>
    public static object IncreasePrice(object product, decimal amount)
    {
        if (product is ProductRecord record)
            return record with { Price = record.Price + amount };

        throw new ArgumentException("Expected: ProductRecord", nameof(product));
    }

    /// <summary>
    /// Given two ProductRecord instances, demonstrate that records compare by
    /// value. Return true if they represent the same product (same Id, Name, Price).
    /// Hint: just use ==  (records do this automatically).
    /// </summary>
    public static bool AreEqual(object a, object b)
    {
        if (a is ProductRecord aProduct && b is ProductRecord bProduct)
            return aProduct == bProduct;

        return false;
    }
}
