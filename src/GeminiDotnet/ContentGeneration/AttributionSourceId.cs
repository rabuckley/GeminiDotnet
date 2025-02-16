using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Identifier for the source contributing to this attribution.
/// </summary>
public sealed record AttributionSourceId
{
    [JsonPropertyName("groundingPassage")]
    public GroundingPassage? GroundingPassage { get; init; }

    [JsonPropertyName("semanticRetrieverChunk")]
    public SemanticRetrieverChunk? SemanticRetrieverChunk { get; init; }
}
