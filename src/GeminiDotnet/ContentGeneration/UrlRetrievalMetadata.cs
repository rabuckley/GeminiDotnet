using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Metadata related to url context retrieval tool.
/// </summary>
public sealed record UrlRetrievalMetadata
{
    /// <summary>
    /// List of url retrieval contexts.
    /// </summary>
    [JsonPropertyName("urlRetrievalContexts")]
    public required IReadOnlyList<UrlRetrievalContext> UrlRetrievalContexts { get; init; }
}
