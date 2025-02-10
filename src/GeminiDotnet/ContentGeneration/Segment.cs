using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Segment of the content.
/// </summary>
public sealed record Segment
{
    /// <summary>
    /// The index of a Part object within its parent Content object.
    /// </summary>
    [JsonPropertyName("partIndex")]
    public required int PartIndex { get; init; }

    /// <summary>
    /// Start index in the given Part, measured in bytes. Offset from the start of the Part, inclusive, starting at zero.
    /// </summary>
    [JsonPropertyName("startIndex")]
    public required int StartIndex { get; init; }

    /// <summary>
    /// End index in the given Part, measured in bytes. Offset from the start of the Part, exclusive, starting at zero.
    /// </summary>
    [JsonPropertyName("endIndex")]
    public required int EndIndex { get; init; }

    /// <summary>
    /// The text corresponding to the segment from the response.x
    /// </summary>
    [JsonPropertyName("text")]
    public required string Text { get; init; }
}