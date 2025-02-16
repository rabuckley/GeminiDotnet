using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// A collection of source attributions for a piece of content.
/// </summary>
public sealed record CitationMetadata
{
    /// <summary>
    /// Citations to sources for a specific response.
    /// </summary>
    [JsonPropertyName("citationSources")]
    public required IReadOnlyCollection<CitationSource> Sources { get; init; }
}
