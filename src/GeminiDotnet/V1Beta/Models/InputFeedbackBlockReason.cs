using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Optional. If set, the input was blocked and no candidates are returned.
/// Rephrase the input.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<InputFeedbackBlockReason>))]
public enum InputFeedbackBlockReason
{
    /// <summary>
    /// Default value. This value is unused.
    /// </summary>
    [JsonStringEnumMemberName("BLOCK_REASON_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Input was blocked due to safety reasons. Inspect
    /// `safety_ratings` to understand which safety category blocked it.
    /// </summary>
    [JsonStringEnumMemberName("SAFETY")]
    Safety,

    /// <summary>
    /// Input was blocked due to other reasons.
    /// </summary>
    [JsonStringEnumMemberName("OTHER")]
    Other,
}

