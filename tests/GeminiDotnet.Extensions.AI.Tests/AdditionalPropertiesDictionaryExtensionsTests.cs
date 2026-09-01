using GeminiDotnet.V1Beta;
using Microsoft.Extensions.AI;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace GeminiDotnet.Extensions.AI;

public sealed class AdditionalPropertiesDictionaryExtensionsTests
{
    [Fact]
    public void TryGetGeminiValue_WithAnAbsentKey_ShouldReturnFalse()
    {
        // Arrange
        var properties = new AdditionalPropertiesDictionary();

        // Act
        var found = properties.TryGetGeminiValue("missing", out int value);

        // Assert
        Assert.False(found);
        Assert.Equal(0, value);
    }

    [Fact]
    public void TryGetGeminiValue_WithANullValue_ShouldReturnFalse()
    {
        // Arrange
        var properties = new AdditionalPropertiesDictionary { ["key"] = null };

        // Act
        var found = properties.TryGetGeminiValue("key", out string? value);

        // Assert
        Assert.False(found);
        Assert.Null(value);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TryGetGeminiValue_WithAValueOfTheWrongType_ShouldReturnFalse(bool asJsonElement)
    {
        // Arrange
        var properties = new AdditionalPropertiesDictionary { ["key"] = MaybeAsJsonElement("not a number", asJsonElement) };

        // Act
        var found = properties.TryGetGeminiValue("key", out int value);

        // Assert
        Assert.False(found);
        Assert.Equal(0, value);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TryGetGeminiValue_WithAnEnum_ShouldReturnIt(bool asJsonElement)
    {
        // Arrange — a persisted history hands the enum back as the JsonElement "OUTCOME_FAILED".
        var properties = new AdditionalPropertiesDictionary
        {
            [GeminiContentProperties.Outcome] = MaybeAsJsonElement(CodeExecutionResultOutcome.Failed, asJsonElement),
        };

        // Act
        var found = properties.TryGetGeminiValue(GeminiContentProperties.Outcome, out CodeExecutionResultOutcome value);

        // Assert
        Assert.True(found);
        Assert.Equal(CodeExecutionResultOutcome.Failed, value);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TryGetGeminiValue_WithAnInt_ShouldReturnIt(bool asJsonElement)
    {
        // Arrange
        var properties = new AdditionalPropertiesDictionary
        {
            [GeminiCitationProperties.PageNumber] = MaybeAsJsonElement(7, asJsonElement),
        };

        // Act
        var found = properties.TryGetGeminiValue(GeminiCitationProperties.PageNumber, out int value);

        // Assert
        Assert.True(found);
        Assert.Equal(7, value);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TryGetGeminiValue_WithAFloat_ShouldReturnIt(bool asJsonElement)
    {
        // Arrange — a CustomMetadata score is a float.
        var properties = new AdditionalPropertiesDictionary { ["score"] = MaybeAsJsonElement(0.5f, asJsonElement) };

        // Act
        var found = properties.TryGetGeminiValue("score", out float value);

        // Assert
        Assert.True(found);
        Assert.Equal(0.5f, value);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TryGetGeminiValue_WithAJsonArray_ShouldReturnIt(bool asJsonElement)
    {
        // Arrange — a multi-valued CustomMetadata entry is a JsonArray.
        var array = new JsonArray("poetry", "war");
        var properties = new AdditionalPropertiesDictionary { ["tags"] = MaybeAsJsonElement(array, asJsonElement) };

        // Act
        var found = properties.TryGetGeminiValue("tags", out JsonArray? value);

        // Assert
        Assert.True(found);
        Assert.NotNull(value);
        Assert.Equal(["poetry", "war"], value.Select(node => node!.GetValue<string>()));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TryGetGeminiValue_WithAString_ShouldReturnIt(bool asJsonElement)
    {
        // Arrange
        var properties = new AdditionalPropertiesDictionary
        {
            [GeminiAdditionalProperties.MetadataFilter] = MaybeAsJsonElement("author = \"Robert Graves\"", asJsonElement),
        };

        // Act
        var found = properties.TryGetGeminiValue(GeminiAdditionalProperties.MetadataFilter, out string? value);

        // Assert
        Assert.True(found);
        Assert.Equal("author = \"Robert Graves\"", value);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TryGetGeminiValue_WithAListOfEnums_ShouldReturnIt(bool asJsonElement)
    {
        // Arrange — Semantic Kernel's PromptExecutionSettings.ToChatOptions() hands the modalities over as
        // a JsonElement.
        var properties = new AdditionalPropertiesDictionary
        {
            [GeminiAdditionalProperties.ResponseModalities] =
                MaybeAsJsonElement(new List<ResponseModality> { ResponseModality.Image }, asJsonElement),
        };

        // Act
        var found = properties.TryGetGeminiValue(
            GeminiAdditionalProperties.ResponseModalities, out List<ResponseModality>? value);

        // Assert
        Assert.True(found);
        Assert.Equal([ResponseModality.Image], value);
    }

    private static object MaybeAsJsonElement<T>(T value, bool asJsonElement)
    {
        return asJsonElement
            ? JsonSerializer.SerializeToElement(value, GeminiJsonUtilities.DefaultOptions)
            : value!;
    }
}
