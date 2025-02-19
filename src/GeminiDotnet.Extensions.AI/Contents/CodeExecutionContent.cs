using Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI.Contents;

/// <summary>
/// Represents the result of a service-side code execution.
/// </summary>
public sealed class CodeExecutionContent : AIContent
{
    /// <summary>
    /// The output of the code execution.
    /// </summary>
    public string? Output { get; init; }

    /// <summary>
    /// The status of the code execution.
    /// </summary>
    public CodeExecutionStatus Status { get; init; }
}
