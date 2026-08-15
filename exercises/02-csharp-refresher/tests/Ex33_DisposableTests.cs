using CSharpExercises;
using Xunit;

namespace CSharpExercises.Tests;

public class Ex33_DisposableTests
{
    [Fact]
    public void ManagedResource_Read_BeforeDispose_Succeeds()
    {
        using var r = new ManagedResource();
        var _ = r.Read(); // should not throw
    }

    [Fact]
    public void ManagedResource_Read_AfterDispose_ThrowsObjectDisposed()
    {
        var r = new ManagedResource();
        r.Dispose();
        Assert.Throws<ObjectDisposedException>(() => r.Read());
    }

    [Fact]
    public void ManagedResource_IsDisposed_TrueAfterDispose()
    {
        var r = new ManagedResource();
        Assert.False(r.IsDisposed);
        r.Dispose();
        Assert.True(r.IsDisposed);
    }

    [Fact]
    public void ManagedResource_DoubleDispose_IsIdempotent()
    {
        var r = new ManagedResource();
        r.Dispose();
        r.Dispose(); // should not throw
    }

    [Fact]
    public async Task AsyncManagedResource_IsDisposed_TrueAfterDisposeAsync()
    {
        var r = new AsyncManagedResource();
        Assert.False(r.IsDisposed);
        await r.DisposeAsync();
        Assert.True(r.IsDisposed);
    }
}
