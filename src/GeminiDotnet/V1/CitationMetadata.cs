using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// A collection of source attributions for a piece of content.
/// </summary>
public sealed record CitationMetadata
{
    /// <summary>
    /// Citations to sources for a specific response.
    /// </summary>
    [JsonPropertyName("citationSources")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<CitationSource>? CitationSources { get; init; }
}

