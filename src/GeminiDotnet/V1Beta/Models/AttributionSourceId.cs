using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Identifier for the source contributing to this attribution.
/// </summary>
public sealed record AttributionSourceId
{
    /// <summary>
    /// Identifier for an inline passage.
    /// </summary>
    [JsonPropertyName("groundingPassage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public GroundingPassageId? GroundingPassage { get; init; }

    /// <summary>
    /// Identifier for a <c>Chunk</c> fetched via Semantic Retriever.
    /// </summary>
    [JsonPropertyName("semanticRetrieverChunk")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SemanticRetrieverChunk? SemanticRetrieverChunk { get; init; }
}

