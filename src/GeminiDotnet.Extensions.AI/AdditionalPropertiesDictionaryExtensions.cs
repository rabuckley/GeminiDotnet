using Microsoft.Extensions.AI;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// Reads the Gemini values this library stores in an additional-properties dictionary, on
/// <see cref="AIContent"/>, <see cref="AIAnnotation"/>, <see cref="ChatOptions"/> or <see cref="AITool"/>.
/// </summary>
public static class AdditionalPropertiesDictionaryExtensions
{
    /// <summary>
    /// Reads a Gemini value from the dictionary, whether the entry holds the value itself or a
    /// <see cref="JsonElement"/> holding it, as a history round-tripped through JSON delivers.
    /// </summary>
    /// <typeparam name="T">The type the key's documentation names.</typeparam>
    /// <param name="properties">The dictionary to read.</param>
    /// <param name="key">
    /// The key to read, one of the constants on <see cref="GeminiContentProperties"/>,
    /// <see cref="GeminiCitationProperties"/> or <see cref="GeminiAdditionalProperties"/>.
    /// </param>
    /// <param name="value">
    /// When this method returns, contains the value stored under <paramref name="key"/>, if it was found;
    /// otherwise, the default value for <typeparamref name="T"/>. This parameter is treated as uninitialized.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="key"/> holds a non-null value that is, or deserializes to,
    /// a <typeparamref name="T"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="properties"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <see cref="AdditionalPropertiesDictionary.TryGetValue{T}"/> does not read through a
    /// <see cref="JsonElement"/>, so after a round trip through JSON it reports every Gemini value as absent.
    /// This method deserializes a <see cref="JsonElement"/> with <see cref="GeminiJsonUtilities.DefaultOptions"/>,
    /// so under Native AOT it reads back any type those options can write.
    /// </remarks>
    public static bool TryGetGeminiValue<T>(
        this IReadOnlyDictionary<string, object?> properties,
        string key,
        [MaybeNullWhen(false)] out T value)
    {
        ArgumentNullException.ThrowIfNull(properties);

        if (properties.TryGetValue(key, out var stored) && stored is not null
            && TryConvertValue<T>(stored, out var converted) && converted is not null)
        {
            value = converted;
            return true;
        }

        value = default;
        return false;
    }

    internal static T? GetValueOrDefault<T>(
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
    internal static T? GetValueOrThrow<T>(
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
        if (value is T t)
        {
            converted = t;
            return true;
        }

        // A value that crossed a JSON boundary (a persisted history, or Semantic Kernel's
        // PromptExecutionSettings.ToChatOptions()) arrives as a JsonElement. Resolving T through the
        // library's default options, rather than JsonContext alone, means that under AOT the read side
        // can read back whatever those documented options can write: JsonContext supplies the V1Beta
        // types and M.E.AI's context supplies the BCL types the citation keys name.
        if (value is JsonElement jsonElement
            && GeminiJsonUtilities.DefaultOptions.TryGetTypeInfo(typeof(T), out var typeInfo)
            && typeInfo is JsonTypeInfo<T> typedInfo)
        {
            try
            {
                converted = jsonElement.Deserialize(typedInfo);
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
