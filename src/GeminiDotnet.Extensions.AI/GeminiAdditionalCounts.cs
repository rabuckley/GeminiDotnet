using Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// Keys for custom properties added to the <see cref="UsageDetails.AdditionalCounts"/> dictionary.
/// </summary>
public static class GeminiAdditionalCounts
{
    /// <summary>
    /// Number of tokens present in tool-use prompt(s).
    /// </summary>
    public const string ToolUsePromptTokenCount = "toolUsePromptTokenCount";
}

