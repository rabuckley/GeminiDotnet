using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace GeminiDotnet;

public sealed class StabilityMarkingTests
{
    private static Type[] PublicTypes => typeof(GeminiClient).Assembly.GetExportedTypes();

    [Fact]
    public void EveryV1BetaTypeIsExperimental()
    {
        var unmarked = PublicTypes
            .Where(t => IsInNamespace(t, "GeminiDotnet.V1Beta"))
            .Where(t => t.DeclaringType is null)
            .Where(t => t.GetCustomAttribute<ExperimentalAttribute>()?.DiagnosticId != "GEMINI001")
            .Select(t => t.FullName);

        Assert.Empty(unmarked);
    }

    [Fact]
    public void NoV1TypeIsExperimental()
    {
        var marked = PublicTypes
            .Where(t => IsInNamespace(t, "GeminiDotnet.V1"))
            .Where(t => t.GetCustomAttribute<ExperimentalAttribute>() is not null)
            .Select(t => t.FullName);

        Assert.Empty(marked);
    }

    [Fact]
    public void TheV1BetaEntryPointIsExperimental()
    {
        Assert.NotNull(typeof(IGeminiClient).GetProperty(nameof(IGeminiClient.V1Beta))!
            .GetCustomAttribute<ExperimentalAttribute>());
        Assert.NotNull(typeof(GeminiClient).GetProperty(nameof(GeminiClient.V1Beta))!
            .GetCustomAttribute<ExperimentalAttribute>());
    }

    private static bool IsInNamespace(Type type, string @namespace) =>
        type.Namespace is { } ns
        && (ns == @namespace || ns.StartsWith($"{@namespace}.", StringComparison.Ordinal));
}
