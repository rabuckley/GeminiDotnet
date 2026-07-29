using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Result of executing the <see cref="V1.ExecutableCode"/>.
/// Generated only when the <see cref="V1.CodeExecution"/> tool is used.
/// </summary>
public sealed record CodeExecutionResult
{
    /// <summary>
    /// Optional. The identifier of the <see cref="V1.ExecutableCode"/> part this result is for.
    /// Only populated if the corresponding <see cref="V1.ExecutableCode"/> has an id.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Id { get; init; }

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

