using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.FunctionCalling;

/// <summary>
/// Enumeration of possible outcomes of the code execution.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<CodeExecutionOutcome>))]
public enum CodeExecutionOutcome
{
    /// <summary>
    /// Unspecified status. This value should not be used.
    /// </summary>
    [JsonStringEnumMemberName("OUTCOME_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Code execution completed successfully.
    /// </summary>
    [JsonStringEnumMemberName("OUTCOME_OK")]
    Ok,

    /// <summary>
    /// Code execution finished but with a failure. stderr should contain the reason.
    /// </summary>
    [JsonStringEnumMemberName("OUTCOME_FAILED")]
    Failed,

    /// <summary>
    /// Code execution ran for too long, and was cancelled. There may or may not be a partial output present.
    /// </summary>
    [JsonStringEnumMemberName("OUTCOME_DEADLINE_EXCEEDED")]
    DeadlineExceeded,
}
