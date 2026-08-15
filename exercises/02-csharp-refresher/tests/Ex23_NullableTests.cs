using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex23_NullableTests
{
    [Fact]
    public void SafeLength_NullString_ReturnsZero()
        => Assert.Equal(0, NullableExercises.SafeLength(null));

    [Fact]
    public void SafeLength_NonNull_ReturnsLength()
        => Assert.Equal(5, NullableExercises.SafeLength("hello"));

    [Fact]
    public void FirstOrNull_ReturnsMatch()
    {
        var result = NullableExercises.FirstOrNull(["a","bb","ccc"], s => s.Length == 2);
        Assert.Equal("bb", result);
    }

    [Fact]
    public void FirstOrNull_NoMatch_ReturnsNull()
    {
        var result = NullableExercises.FirstOrNull(["a","b"], s => s.Length == 5);
        Assert.Null(result);
    }

    [Fact]
    public void GetCity_ReturnsCity_WhenAllNonNull()
    {
        var user = new User { Address = new Address { City = "London" } };
        Assert.Equal("London", NullableExercises.GetCity(user));
    }

    [Fact]
    public void GetCity_ReturnsUnknown_WhenUserIsNull()
        => Assert.Equal("Unknown", NullableExercises.GetCity(null));

    [Fact]
    public void GetCity_ReturnsUnknown_WhenCityIsNull()
    {
        var user = new User { Address = new Address { City = null } };
        Assert.Equal("Unknown", NullableExercises.GetCity(user));
    }

    [Fact]
    public void GetOrAdd_ComputesAndCachesValue()
    {
        var cache = new Dictionary<string,int>();
        var v1    = NullableExercises.GetOrAdd(cache, "x", k => 42);
        var v2    = NullableExercises.GetOrAdd(cache, "x", k => 999);
        Assert.Equal(42, v1);
        Assert.Equal(42, v2); // cached, factory not called again
    }
}
