using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// A citation to a source for a portion of a specific response.
/// </summary>
public sealed record CitationSource
{
    /// <summary>
    /// Optional. End of the attributed segment, exclusive.
    /// </summary>
    [JsonPropertyName("endIndex")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? EndIndex { get; init; }

    /// <summary>
    /// Optional. License for the GitHub project that is attributed as a source for segment.
    /// License info is required for code citations.
    /// </summary>
    [JsonPropertyName("license")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? License { get; init; }

    /// <summary>
    /// Optional. Start of segment of the response that is attributed to this source.
    /// Index indicates the start of the segment, measured in bytes.
    /// </summary>
    [JsonPropertyName("startIndex")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? StartIndex { get; init; }

    /// <summary>
    /// Optional. URI that is attributed as a source for a portion of the text.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Uri { get; init; }
}

