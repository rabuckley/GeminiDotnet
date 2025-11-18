using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Result of executing the <see cref="V1Beta.ExecutableCode"/>.
/// Only generated when using the <see cref="V1Beta.CachedContents.CodeExecution"/>, and always follows a <c>part</c>
/// containing the <see cref="V1Beta.ExecutableCode"/>.
/// </summary>
public sealed record CodeExecutionResult
{
    /// <summary>
    /// Required. Outcome of the code execution.
    /// </summary>
    [JsonPropertyName("outcome")]
    public required CodeExecutionResultOutcome Outcome { get; init; }

    /// <summary>
    /// Optional. Contains stdout when code execution is successful, stderr or other
    /// description otherwise.
    /// </summary>
    [JsonPropertyName("output")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Output { get; init; }
}

