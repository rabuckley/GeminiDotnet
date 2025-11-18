using GeminiDotnet.V1Beta;
using Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI.Contents;

/// <summary>
/// Represents service-side executable code content.
/// </summary>
public sealed class ExecutableCodeContent : AIContent
{
    /// <summary>
    /// The language of the code.
    /// </summary>
    public required ExecutableCodeLanguage Language { get; set; }

    /// <summary>
    /// The code the model will execute.
    /// </summary>
    public required string Code { get; set; }
}
