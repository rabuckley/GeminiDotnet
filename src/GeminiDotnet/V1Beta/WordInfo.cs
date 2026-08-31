using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Information about a single recognized word.
/// </summary>
public sealed record WordInfo
{
    /// <summary>
    /// Optional. End offset in time of the word relative to the start of the audio.
    /// </summary>
    [JsonPropertyName("endOffset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? EndOffset { get; init; }

    /// <summary>
    /// Optional. Start offset in time of the word relative to the start of the audio.
    /// </summary>
    [JsonPropertyName("startOffset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? StartOffset { get; init; }

    /// <summary>
    /// Required. Transcript of the word.
    /// </summary>
    [JsonPropertyName("word")]
    public required string Word { get; init; }
}

