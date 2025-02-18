using System.Text.Json.Serialization;

namespace GeminiDotnet.Extensions.AI.Contents;

/// <summary>
/// Represents the outcome of a code execution.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<CodeExecutionStatus>))]
public enum CodeExecutionStatus
{
    /// <summary>
    /// No status.
    /// </summary>
    [JsonStringEnumMemberName("none")]
    None,

    /// <summary>
    /// The code execution was successful.
    /// </summary>
    [JsonStringEnumMemberName("success")]
    Success,

    /// <summary>
    /// The code execution failed.
    /// </summary>
    [JsonStringEnumMemberName("error")]
    Error,
    
    /// <summary>
    /// The code execution timed out.
    /// </summary>
    [JsonStringEnumMemberName("timeout")]
    Timeout
}
