using GeminiDotnet.Extensions.AI.Contents;
using Microsoft.Extensions.AI;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// Provides utility methods for working with JSON in Gemini.Extensions.AI.
/// </summary>
public static class GeminiJsonUtilities
{
    /// <summary>
    /// Gets a <see cref="JsonSerializerOptions" /> singleton with the default settings for Gemini.Extensions.AI including registered custom <see cref="AIContent"/> types.
    /// <remarks>
    /// Note: this extends the types provided by <see cref="AIJsonUtilities.DefaultOptions"/>.
    /// </remarks>
    /// </summary>
    public static JsonSerializerOptions DefaultOptions { get; } = CreateDefaultOptions();

    private static JsonSerializerOptions CreateDefaultOptions()
    {
        var options = new JsonSerializerOptions(AIJsonUtilities.DefaultOptions)
        {
            TypeInfoResolver = JsonTypeInfoResolver.Combine(
                JsonContext.Default,
                AIJsonUtilities.DefaultOptions.TypeInfoResolver
            )
        };

        options.AddAIContentType<ExecutableCodeContent>("executable_code");
        options.AddAIContentType<CodeExecutionContent>("code_execution");

        options.MakeReadOnly();

        return options;
    }
}
