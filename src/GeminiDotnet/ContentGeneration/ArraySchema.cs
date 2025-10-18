using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

public sealed record ArraySchema : Schema
{
    private const string MaxItemsPropertyName = "maxItems";

    private const string MinItemsPropertyName = "minItems";

    private const string ItemsPropertyName = "items";

    /// <summary>
    /// Maximum number of the elements
    /// </summary>
    [JsonPropertyName(MaxItemsPropertyName)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxItems { get; init; }

    /// <summary>
    /// Minimum number of the elements
    /// </summary>
    [JsonPropertyName(MinItemsPropertyName)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MinItems { get; init; }

    /// <summary>
    /// Schema of the elements.
    /// </summary>
    [JsonPropertyName(ItemsPropertyName)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Schema? Items { get; init; }

    internal static ArraySchema Create(JsonElement element, SchemaInfo schemaInfo, JsonElement rootElement)
    {
        var items = element.TryGetProperty(ItemsPropertyName, out var itemsElement)
            ? FromJsonElement(itemsElement, rootElement)
            : null;

        return new ArraySchema
        {
            Format = schemaInfo.Format,
            Description = schemaInfo.Description,
            Nullable = schemaInfo.Nullable,
            MaxItems = element.TryGetProperty(MaxItemsPropertyName, out var maxItems) ? maxItems.GetInt32() : null,
            MinItems = element.TryGetProperty(MinItemsPropertyName, out var minItems) ? minItems.GetInt32() : null,
            Items = items,
        };
    }
}
