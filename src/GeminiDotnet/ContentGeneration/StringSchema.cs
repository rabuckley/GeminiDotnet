using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

public sealed record StringSchema : Schema
{
    private const string EnumValuesPropertyName = "enumValues";

    /// <summary>
    /// Possible values of the element of type <c>STRING</c> with enum format.
    /// </summary>
    [JsonPropertyName(EnumValuesPropertyName)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<string>? EnumValues { get; init; }

    internal static StringSchema Create(JsonElement element, SchemaInfo schemaInfo)
    {
        var enumValues = element.TryGetProperty(EnumValuesPropertyName, out var enumValuesElement)
            ? EnumValuesToList(enumValuesElement)
            : null;

        return new StringSchema
        {
            Format = schemaInfo.Format,
            Description = schemaInfo.Description,
            Nullable = schemaInfo.Nullable,
            EnumValues = enumValues,
        };

        static List<string> EnumValuesToList(JsonElement element)
        {
            List<string> enumValues = [];

            foreach (var item in element.EnumerateArray())
            {
                enumValues.Add(item.GetString() ?? throw new JsonException("Enum value must be a string"));
            }

            return enumValues;
        }
    }
}