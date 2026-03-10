using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Required. Outcome of the code execution.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<CodeExecutionResultOutcome>))]
public enum CodeExecutionResultOutcome
{
    /// <summary>
    /// Unspecified status. This value should not be used.
    /// </summary>
    [JsonStringEnumMemberName("OUTCOME_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Code execution completed successfully. <c>output</c> contains the stdout, if
    /// any.
    /// </summary>
    [JsonStringEnumMemberName("OUTCOME_OK")]
    Ok,

    /// <summary>
    /// Code execution failed. <c>output</c> contains the stderr and stdout, if any.
    /// </summary>
    [JsonStringEnumMemberName("OUTCOME_FAILED")]
    Failed,

    /// <summary>
    /// Code execution ran for too long, and was cancelled. There may or may not
    /// be a partial <c>output</c> present.
    /// </summary>
    [JsonStringEnumMemberName("OUTCOME_DEADLINE_EXCEEDED")]
    DeadlineExceeded,
}

