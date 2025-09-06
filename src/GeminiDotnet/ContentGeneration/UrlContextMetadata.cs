using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Metadata related to url context retrieval tool.
/// </summary>
public sealed record UrlContextMetadata
{
    /// <summary>
    /// List of url context.
    /// </summary>
    [JsonPropertyName("urlMetadata")]
    public required IReadOnlyList<UrlMetadata> UrlMetadata { get; init; }
}
