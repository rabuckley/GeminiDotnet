using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.FunctionCalling;

/// <summary>
/// Code generated by the model that is meant to be executed, and the result returned to the model.
/// Only generated when using the CodeExecution tool, in which the code will be automatically executed, and a
/// corresponding <see cref="CodeExecutionResult"/> will also be generated.
/// </summary>
public sealed record ExecutableCode
{
    /// <summary>
    /// Programming language of the code.
    /// </summary>
    [JsonPropertyName("language")]
    public required string Language { get; init; }

    /// <summary>
    /// The code to be executed.
    /// </summary>
    [JsonPropertyName("code")]
    public required string Code { get; init; }
}
