using GeminiDotnet.Extensions.AI.Contents;
using Microsoft.Extensions.AI;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace GeminiDotnet.Extensions.AI;

public static class GeminiJsonUtilities
{
    public static JsonSerializerOptions CreateDefaultOptions()
    {
        var options = new JsonSerializerOptions(AIJsonUtilities.DefaultOptions);

        options.TypeInfoResolver = JsonTypeInfoResolver.Combine(
            JsonContext.Default,
            AIJsonUtilities.DefaultOptions.TypeInfoResolver
        );
        
        options.AddAIContentType<ExecutableCodeContent>("executable_code");
        options.AddAIContentType<CodeExecutionContent>("code_execution");

        options.MakeReadOnly();

        return options;
    }
}
