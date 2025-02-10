using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Specifies the reason why the prompt was blocked.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<BlockReason>))]
public enum BlockReason
{
    /// <summary>
    /// Default value. This value is unused.
    /// </summary>
    [JsonStringEnumMemberName("BLOCK_REASON_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Prompt was blocked due to safety reasons. Inspect <see cref="PromptFeedback.SafetyRatings"/> to understand which safety category blocked it.
    /// </summary>
    [JsonStringEnumMemberName("SAFETY")]
    Safety,

    /// <summary>
    /// Prompt was blocked due to unknown reasons.
    /// </summary>
    [JsonStringEnumMemberName("OTHER")]
    Other,

    /// <summary>
    /// Prompt was blocked due to the terms which are included from the terminology blocklist.
    /// </summary>
    [JsonStringEnumMemberName("BLOCKLIST")]
    Blocklist,

    /// <summary>
    /// Prompt was blocked due to prohibited content.
    /// </summary>
    [JsonStringEnumMemberName("PROHIBITED_CONTENT")]
    ProhibitedContent,

    /// <summary>
    /// Candidates blocked due to unsafe image generation content.
    /// </summary>
    [JsonStringEnumMemberName("IMAGE_SAFETY")]
    ImageSafety,
}