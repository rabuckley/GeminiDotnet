using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.FunctionCalling;

/// <summary>
/// Result of executing the <see cref="ExecutableCode"/>.
/// </summary>
public sealed record CodeExecutionResult
{
    /// <summary>
    /// Outcome of the code execution.
    /// </summary>
    [JsonPropertyName("outcome")]
    public required CodeExecutionOutcome Outcome { get; init; }

    /// <summary>
    /// Contains stdout when code execution is successful, stderr or other description otherwise.
    /// </summary>
    [JsonPropertyName("output")]
    public required string Output { get; init; }
}
