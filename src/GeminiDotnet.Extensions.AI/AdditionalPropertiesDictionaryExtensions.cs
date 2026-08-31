using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace GeminiDotnet.Extensions.AI;

internal static class AdditionalPropertiesDictionaryExtensions
{
    public static T? GetValueOrDefault<T>(
        this IReadOnlyDictionary<string, object?> dictionary,
        string key)
    {
        return dictionary.TryGetValue(key, out var value) && value is not null
            && TryConvertValue<T>(value, out var converted)
            ? converted
            : default;
    }

    /// <summary>
    /// Reads the value stored under <paramref name="key"/>, reporting a value of the wrong type rather
    /// than dropping it. Use this where the caller cannot see that the value went missing.
    /// </summary>
    /// <returns>
    /// The value, or <see langword="default"/> when <paramref name="key"/> is absent or holds
    /// <see langword="null"/>.
    /// </returns>
    /// <exception cref="GeminiMappingException">
    /// <paramref name="key"/> holds a value that is not a <typeparamref name="T"/> and cannot be
    /// deserialized into one.
    /// </exception>
    public static T? GetValueOrThrow<T>(
        this IReadOnlyDictionary<string, object?> dictionary,
        string key,
        string fromPropertyName,
        string toPropertyName)
    {
        if (!dictionary.TryGetValue(key, out var value) || value is null)
        {
            return default;
        }

        if (!TryConvertValue<T>(value, out var converted))
        {
            GeminiMappingException.Throw(
                fromPropertyName: $"{fromPropertyName}[\"{key}\"]",
                toPropertyName: toPropertyName,
                reason: $"Expected a value of type {typeof(T)}, but found {value.GetType()}.");
        }

        return converted;
    }

    private static bool TryConvertValue<T>(object value, out T? converted)
    {
        // Direct type match
        if (value is T t)
        {
            converted = t;
            return true;
        }

        // Handle JsonElement (e.g., when settings are roundtripped through JSON serialization,
        // as done by Semantic Kernel's PromptExecutionSettings.ToChatOptions())
        if (value is JsonElement jsonElement
            // Try to get type info from our source-generated JsonContext (AOT-safe)
            && JsonContext.Default.GetTypeInfo(typeof(T)) is JsonTypeInfo<T> typeInfo)
        {
            try
            {
                converted = jsonElement.Deserialize(typeInfo);
                return true;
            }
            catch (JsonException)
            {
                // Falls through to the failure below: the element does not hold a T.
            }
        }

        converted = default;
        return false;
    }
}
