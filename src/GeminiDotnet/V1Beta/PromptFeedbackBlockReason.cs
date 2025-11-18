using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. If set, the prompt was blocked and no candidates are returned.
/// Rephrase the prompt.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<PromptFeedbackBlockReason>))]
public enum PromptFeedbackBlockReason
{
    /// <summary>
    /// Default value. This value is unused.
    /// </summary>
    [JsonStringEnumMemberName("BLOCK_REASON_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Prompt was blocked due to safety reasons. Inspect `safety_ratings`
    /// to understand which safety category blocked it.
    /// </summary>
    [JsonStringEnumMemberName("SAFETY")]
    Safety,

    /// <summary>
    /// Prompt was blocked due to unknown reasons.
    /// </summary>
    [JsonStringEnumMemberName("OTHER")]
    Other,

    /// <summary>
    /// Prompt was blocked due to the terms which are included from the
    /// terminology blocklist.
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

