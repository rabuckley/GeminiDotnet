using Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI;

internal static class AdditionalPropertiesDictionaryExtensions
{
    public static T? GetValueOrDefault<T>(
        this AdditionalPropertiesDictionary dictionary,
        string key)
    {
        return dictionary.TryGetValue(key, out var obj) && obj is T t
            ? t
            : default;
    }
}
