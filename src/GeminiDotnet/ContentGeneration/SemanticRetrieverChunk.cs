using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Identifier for a Chunk retrieved via Semantic Retriever specified in the GenerateAnswerRequest using SemanticRetrieverConfig.
/// </summary>
public sealed record SemanticRetrieverChunk
{
    /// <summary>
    /// Name of the source matching the request's SemanticRetrieverConfig.source. Example: corpora/123 or corpora/123/documents/abc
    /// </summary>
    [JsonPropertyName("source")]
    public required string Source { get; init; }

    /// <summary>
    /// Name of the Chunk containing the attributed text. Example: corpora/123/documents/abc/chunks/xyz
    /// </summary>
    [JsonPropertyName("chunk")]
    public required string Chunk { get; init; }
}