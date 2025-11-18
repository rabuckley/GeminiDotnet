using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// The reason content was blocked during request processing.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ContentFilterReason>))]
public enum ContentFilterReason
{
    /// <summary>
    /// A blocked reason was not specified.
    /// </summary>
    [JsonStringEnumMemberName("BLOCKED_REASON_UNSPECIFIED")]
    BlockedReasonUnspecified,

    /// <summary>
    /// Content was blocked by safety settings.
    /// </summary>
    [JsonStringEnumMemberName("SAFETY")]
    Safety,

    /// <summary>
    /// Content was blocked, but the reason is uncategorized.
    /// </summary>
    [JsonStringEnumMemberName("OTHER")]
    Other,
}

