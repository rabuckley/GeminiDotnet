using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace GeminiDotnet.Testing;

/// <summary>
/// Provides methods for comparing JSON documents as text.
/// </summary>
public static class JsonText
{
    private static readonly JsonSerializerOptions IndentedOptions = new() { WriteIndented = true };

    /// <summary>
    /// Rewrites a JSON document indented and with the properties of every object sorted by name.
    /// </summary>
    /// <param name="json">The JSON document to rewrite.</param>
    /// <returns>
    /// The normalized document, which is the same for two documents that differ only in whitespace or
    /// property order.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="json"/> is <see langword="null"/>.</exception>
    /// <exception cref="JsonException"><paramref name="json"/> is not valid JSON.</exception>
    public static string Normalize([StringSyntax(StringSyntaxAttribute.Json)] string json)
    {
        ArgumentNullException.ThrowIfNull(json);

        var sorted = Sort(JsonNode.Parse(json));
        return sorted?.ToJsonString(IndentedOptions) ?? "null";
    }

    private static JsonNode? Sort(JsonNode? node)
    {
        return node switch
        {
            JsonObject obj => new JsonObject(obj
                .OrderBy(property => property.Key, StringComparer.Ordinal)
                .Select(property => KeyValuePair.Create(property.Key, Sort(property.Value)))),
            JsonArray array => new JsonArray([.. array.Select(Sort)]),
            null => null,
            _ => node.DeepClone(),
        };
    }
}
