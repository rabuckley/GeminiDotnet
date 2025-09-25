using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

[JsonConverter(typeof(JsonStringEnumConverter<FinishReason>))]
public enum FinishReason
{
    /// <summary>
    /// Unknown finish reason.
    /// </summary>
    [JsonStringEnumMemberName("FINISH_REASON_UNKNOWN")]
    Unknown,

    /// <summary>
    /// Unspecified finish reason.
    /// </summary>
    [JsonStringEnumMemberName("FINISH_REASON_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Natural stop point of the model or provided stop sequence.
    /// </summary>
    [JsonStringEnumMemberName("STOP")]
    Stop,

    /// <summary>
    /// The maximum number of tokens as specified in the request was reached.
    /// </summary>
    [JsonStringEnumMemberName("MAX_TOKENS")]
    MaxTokens,

    /// <summary>
    /// The token generation was stopped because the response was flagged for safety reasons.
    /// NOTE: When streaming, the candidate's content will be empty if content filters blocked the output.
    /// </summary>
    [JsonStringEnumMemberName("SAFETY")]
    Safety,

    /// <summary>
    /// The token generation was stopped because the response was flagged for unauthorized citations.
    /// </summary>
    [JsonStringEnumMemberName("RECITATION")]
    Recitation,

    /// <summary>
    /// The prompt was blocked because it was flagged for containing the prohibited contents, usually CSAM.
    /// </summary>
    [JsonStringEnumMemberName("PROHIBITED_CONTENT")]
    ProhibitedContent,

    /// <summary>
    /// The reason for blocking the prompt is unspecified.
    /// </summary>
    [JsonStringEnumMemberName("BLOCKED_REASON_UNSPECIFIED")]
    BlockedReasonUnspecified,

    /// <summary>
    /// The token generation was stopped because the response was flagged for Sensitive Personally Identifiable Information (SPII) content..
    /// </summary>
    [JsonStringEnumMemberName("SPII")]
    SPII,

    /// <summary>
    /// All other reasons that stopped token generation.
    ///</summary>
    [JsonStringEnumMemberName("OTHER")]
    Other
}
