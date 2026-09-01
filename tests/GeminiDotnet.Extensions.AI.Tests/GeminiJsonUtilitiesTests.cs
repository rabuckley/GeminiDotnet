using GeminiDotnet.V1Beta;
using Microsoft.Extensions.AI;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;

namespace GeminiDotnet.Extensions.AI;

public sealed class GeminiJsonUtilitiesTests
{
    [Theory]
    [InlineData(typeof(string))]
    [InlineData(typeof(int))]
    [InlineData(typeof(float))]
    [InlineData(typeof(JsonElement))]
    [InlineData(typeof(JsonArray))]
    [InlineData(typeof(AdditionalPropertiesDictionary))]
    [InlineData(typeof(ToolType))]
    [InlineData(typeof(CodeExecutionResultOutcome))]
    [InlineData(typeof(ThinkingConfiguration))]
    [InlineData(typeof(ImageConfiguration))]
    [InlineData(typeof(List<ResponseModality>))]
    [InlineData(typeof(IEnumerable<ResponseModality>))]
    public void DefaultOptions_WithoutReflection_ShouldResolveEveryDocumentedPropertyType(System.Type type)
    {
        // Arrange — the test host runs with reflection enabled, so DefaultOptions ends in a
        // DefaultJsonTypeInfoResolver that answers for any type. Under Native AOT only the two
        // source-generated contexts remain, so every type a GeminiContentProperties,
        // GeminiCitationProperties or GeminiAdditionalProperties key names has to be registered with one
        // of them for TryGetGeminiValue to read it back.
        var options = CreateOptionsWithoutReflection();

        // Act
        var resolved = options.TryGetTypeInfo(type, out _);

        // Assert
        Assert.True(resolved);
    }

    private static JsonSerializerOptions CreateOptionsWithoutReflection()
    {
        // The same chain GeminiJsonUtilities builds, minus the reflection resolver M.E.AI appends when
        // reflection is enabled. AddAIContentType wraps the chain in a modifier resolver that hides its
        // members, so it is rebuilt here rather than filtered.
        var sourceGenerated = AIJsonUtilities.DefaultOptions.TypeInfoResolverChain
            .Where(resolver => resolver is not DefaultJsonTypeInfoResolver)
            .Prepend(JsonContext.Default);

        var options = new JsonSerializerOptions(GeminiJsonUtilities.DefaultOptions)
        {
            TypeInfoResolver = JsonTypeInfoResolver.Combine(sourceGenerated.ToArray()),
        };

        Assert.False(options.TryGetTypeInfo(typeof(UnregisteredType), out _));

        return options;
    }

    private sealed class UnregisteredType;
}
