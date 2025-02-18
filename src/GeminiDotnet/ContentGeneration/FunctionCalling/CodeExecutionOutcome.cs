using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.FunctionCalling;

[JsonConverter(typeof(JsonStringEnumConverter<CodeExecutionOutcome>))]
public enum CodeExecutionOutcome
{
    [JsonStringEnumMemberName("OUTCOME_UNSPECIFIED")]
    Unspecified,

    [JsonStringEnumMemberName("OUTCOME_OK")]
    Ok,

    [JsonStringEnumMemberName("OUTCOME_FAILED")]
    Failed,

    [JsonStringEnumMemberName("OUTCOME_DEADLINE_EXCEEDED")]
    DeadlineExceeded,
}
