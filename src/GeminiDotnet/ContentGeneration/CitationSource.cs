using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// A citation to a source for a portion of a specific response.
/// </summary>
public sealed record CitationSource
{
    /// <summary>
    /// Start of segment of the response that is attributed to this source. Index indicates the start of the segment, measured in bytes.
    /// </summary>
    [JsonPropertyName("startIndex")]
    public int? StartIndex { get; init; }

    /// <summary>
    /// End of the attributed segment, exclusive.
    /// </summary>
    [JsonPropertyName("endIndex")]
    public int? EndIndex { get; init; }

    /// <summary>
    /// URI that is attributed as a source for a portion of the text.
    /// </summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; init; }

    /// <summary>
    /// License for the GitHub project that is attributed as a source for segment. License info is required for code citations.
    /// </summary>
    [JsonPropertyName("license")]
    public string? License { get; init; }
}