using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

public sealed record ObjectSchema : Schema
{
    private const string PropertiesPropertyName = "properties";

    private const string RequiredPropertiesPropertyName = "required";

    /// <summary>
    /// Properties of the object.
    /// </summary>
    [JsonPropertyName(PropertiesPropertyName)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IDictionary<string, Schema>? Properties { get; init; }

    /// <summary>
    /// Required properties of the object.
    /// </summary>
    [JsonPropertyName(RequiredPropertiesPropertyName)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<string>? RequiredProperties { get; init; }

    internal static ObjectSchema Create(JsonElement element, SchemaInfo schemaInfo, JsonElement rootElement, HashSet<string> resolvedReferences)
    {
        var properties = element.TryGetProperty(PropertiesPropertyName, out var propertiesElement)
            ? propertiesElement.EnumerateObject().ToDictionary(
                kvp => kvp.Name,
                kvp => FromJsonElement(kvp.Value, rootElement, resolvedReferences))
            : null;

        var requiredProperties = element.TryGetProperty(RequiredPropertiesPropertyName, out var requiredElement)
            ? CreateRequiredProperties(requiredElement)
            : null;

        return new ObjectSchema
        {
            Format = schemaInfo.Format,
            Description = schemaInfo.Description,
            Nullable = schemaInfo.Nullable,
            Properties = properties,
            RequiredProperties = requiredProperties,
        };

        static IEnumerable<string> CreateRequiredProperties(JsonElement requiredElement)
        {
            return requiredElement.EnumerateArray().Select(e =>
                e.GetString() ?? throw new JsonException($"'{RequiredPropertiesPropertyName}' items must be strings"));
        }
    }
}
