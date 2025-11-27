using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Identifier for a <c>Chunk</c> retrieved via Semantic Retriever specified in the
/// <see cref="V1Beta.Models.GenerateAnswerRequest"/> using <see cref="V1Beta.Models.SemanticRetrieverConfig"/>.
/// </summary>
public sealed record SemanticRetrieverChunk
{
    /// <summary>
    /// Output only. Name of the <c>Chunk</c> containing the attributed text.
    /// Example: <c>corpora/123/documents/abc/chunks/xyz</c>
    /// </summary>
    [JsonPropertyName("chunk")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Chunk { get; init; }

    /// <summary>
    /// Output only. Name of the source matching the request's
    /// <c>SemanticRetrieverConfig.source</c>. Example: <c>corpora/123</c> or
    /// <c>corpora/123/documents/abc</c>
    /// </summary>
    [JsonPropertyName("source")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Source { get; init; }
}

