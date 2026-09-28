using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Speech metadata for <c>text</c> parts.
/// </summary>
public sealed record SpeechMetadata
{
    /// <summary>
    /// Optional. Optional speaker name for multi-speaker synthesis.
    /// </summary>
    [JsonPropertyName("speaker")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Speaker { get; init; }

    /// <summary>
    /// Optional. Optional style instruction for the speech synthesis.
    /// </summary>
    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Style { get; init; }
}

