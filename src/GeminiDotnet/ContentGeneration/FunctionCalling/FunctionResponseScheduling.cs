using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.FunctionCalling;

/// <summary>
/// Specifies how the response should be scheduled in the conversation.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<FunctionResponseScheduling>))]
public enum FunctionResponseScheduling
{
    /// <summary>
    /// This value is unused.
    /// </summary>
    [JsonStringEnumMemberName("SCHEDULING_UNSPECIFIED")]
    SchedulingUnspecified,

    /// <summary>
    /// Only add the result to the conversation context, do not interrupt or trigger generation.
    /// </summary>
    [JsonStringEnumMemberName("SILENT")]
    Silent,

    /// <summary>
    /// Add the result to the conversation context, and prompt to generate output without interrupting ongoing generation.
    /// </summary>
    [JsonStringEnumMemberName("WHEN_IDLE")]
    WhenIdle,

    /// <summary>
    /// Add the result to the conversation context, interrupt ongoing generation and prompt to generate output.
    /// </summary>
    [JsonStringEnumMemberName("INTERRUPT")]
    Interrupt,
}
