using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI;

internal static class AdditionalPropertiesDictionaryExtensions
{
    public static T? GetValueOrDefault<T>(
        this AdditionalPropertiesDictionary dictionary,
        string key)
    {
        if (!dictionary.TryGetValue(key, out var obj))
        {
            return default;
        }

        // Direct type match
        if (obj is T t)
        {
            return t;
        }

        // Handle JsonElement (e.g., when settings are roundtripped through JSON serialization,
        // as done by Semantic Kernel's PromptExecutionSettings.ToChatOptions())
        if (obj is JsonElement jsonElement)
        {
            // Try to get type info from our source-generated JsonContext (AOT-safe)
            if (JsonContext.Default.GetTypeInfo(typeof(T)) is JsonTypeInfo<T> typeInfo)
            {
                try
                {
                    return jsonElement.Deserialize(typeInfo);
                }
                catch (JsonException)
                {
                    return default;
                }
            }
        }

        return default;
    }
}
