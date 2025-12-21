using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Segment of the content.
/// </summary>
public sealed record Segment
{
    /// <summary>
    /// End index in the given Part, measured in bytes. Offset from the start of
    /// the Part, exclusive, starting at zero.
    /// </summary>
    [JsonPropertyName("endIndex")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? EndIndex { get; init; }

    /// <summary>
    /// The index of a Part object within its parent Content object.
    /// </summary>
    [JsonPropertyName("partIndex")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? PartIndex { get; init; }

    /// <summary>
    /// Start index in the given Part, measured in bytes. Offset from the start of
    /// the Part, inclusive, starting at zero.
    /// </summary>
    [JsonPropertyName("startIndex")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? StartIndex { get; init; }

    /// <summary>
    /// The text corresponding to the segment from the response.
    /// </summary>
    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Text { get; init; }
}

