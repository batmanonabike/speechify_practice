using System.Reflection;
using System.Runtime.CompilerServices;

namespace Speechify.Billing.Tests;

/// <summary>
/// Structural constraints that a passing implementation must satisfy.
/// </summary>
/// <remarks>
/// These exist because the equivalence tests alone can be satisfied by wrapping
/// the legacy engine, or by reintroducing the very static cache the exercise is
/// about removing. They are cheap and blunt on purpose.
/// </remarks>
public class DesignTests
{
    private static readonly Assembly Target = typeof(BillingComposition).Assembly;

    /// <summary>
    /// Catches actually USING a legacy type, which is the shortcut worth blocking.
    /// </summary>
    /// <remarks>
    /// The compiler drops assembly references that no code touches, so merely adding
    /// the ProjectReference and not using it slips past this. That is fine: an unused
    /// reference changes nothing. Construct or call anything in the legacy assembly
    /// and it shows up here immediately.
    /// </remarks>
    [Fact]
    public void BillingAssembly_DoesNotDependOnTheLegacyAssembly()
    {
        string[] referenced = Target
            .GetReferencedAssemblies()
            .Select(a => a.Name ?? "")
            .ToArray();

        Assert.DoesNotContain("Speechify.Billing.Legacy", referenced);
    }

    [Fact]
    public void BillingAssembly_HasNoMutableStaticState()
    {
        var offenders = new List<string>();

        foreach (Type type in Target.GetTypes())
        {
            if (IsCompilerGenerated(type))
            {
                continue;
            }

            FieldInfo[] fields = type.GetFields(
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            foreach (FieldInfo field in fields)
            {
                // const is fine, readonly is fine. A writable static is not.
                if (field.IsLiteral || field.IsInitOnly)
                {
                    continue;
                }

                if (field.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false))
                {
                    continue;
                }

                offenders.Add($"{type.FullName}.{field.Name}");
            }
        }

        Assert.True(
            offenders.Count == 0,
            "Mutable static state defeats per-instance caching and is not thread safe. Found: "
                + string.Join(", ", offenders));
    }

    [Fact]
    public void Create_ReturnsAnIndependentServiceEachTime()
    {
        var api = new FakeRateApi();
        var clock = new FixedTimeProvider(LegacyStateIsolated.Weekday);

        IBillingService first = BillingComposition.Create(api, clock, TimeSpan.FromMinutes(5));
        IBillingService second = BillingComposition.Create(api, clock, TimeSpan.FromMinutes(5));

        Assert.NotNull(first);
        Assert.NotNull(second);
        Assert.NotSame(first, second);
    }

    private static bool IsCompilerGenerated(Type type)
    {
        if (type.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false))
        {
            return true;
        }

        // Lambda caches and iterator state machines live in <>c-style nested types.
        return (type.FullName ?? type.Name).Contains('<', StringComparison.Ordinal);
    }
}
