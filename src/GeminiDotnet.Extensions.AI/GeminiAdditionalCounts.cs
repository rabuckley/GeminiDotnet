using Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// Keys for custom properties added to the <see cref="UsageDetails.AdditionalCounts"/> dictionary.
/// </summary>
public static class GeminiAdditionalCounts
{
    /// <summary>
    /// The number of tokens in tool-use prompts.
    /// </summary>
    /// <remarks>
    /// This count is included in <see cref="UsageDetails.InputTokenCount"/>. Do not add it again.
    /// </remarks>
    public const string ToolUsePromptTokenCount = "toolUsePromptTokenCount";
}

