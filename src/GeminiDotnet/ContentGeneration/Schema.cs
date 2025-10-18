using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// The Schema object allows the definition of input and output data types. These types can be objects, but also primitives and arrays. Represents a select subset of an <see href="https://spec.openapis.org/oas/v3.0.3#schema">OpenAPI 3.0 schema object</see>.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = TypePropertyName)]
[JsonDerivedType(typeof(StringSchema), "STRING")]
[JsonDerivedType(typeof(NumberSchema), "NUMBER")]
[JsonDerivedType(typeof(IntegerSchema), "INTEGER")]
[JsonDerivedType(typeof(BooleanSchema), "BOOLEAN")]
[JsonDerivedType(typeof(ArraySchema), "ARRAY")]
[JsonDerivedType(typeof(ObjectSchema), "OBJECT")]
public abstract record Schema
{
    private const string TypePropertyName = "type";

    private const string FormatPropertyName = "format";

    private const string DescriptionPropertyName = "description";

    private const string NullablePropertyName = "nullable";

    /// <summary>
    /// The format of the data. This is used only for primitive datatypes.
    /// </summary>
    [JsonPropertyName(FormatPropertyName)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Format { get; init; }

    /// <summary>
    /// A brief description of the parameter. This could contain examples of use. Parameter description may be formatted as Markdown.
    /// </summary>
    [JsonPropertyName(DescriptionPropertyName)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; init; }

    /// <summary>
    /// Indicates if the value may be null.
    /// </summary>
    [JsonPropertyName(NullablePropertyName)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Nullable { get; init; }

    /// <summary>
    /// Creates a <see cref="Schema"/> from the specified <paramref name="node" />
    /// </summary>
    /// <param name="node">The JSON node to create the schema from.</param>
    /// <returns>A <see cref="Schema"/> instance representing the supplied <param name="node" />.</returns>
    /// <exception cref="JsonException">Thrown if the <paramref name="node" /> cannot be converted to a <see cref="Schema" />.</exception>
    public static Schema FromJsonNode(JsonNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        return FromJsonElement(node.Deserialize(JsonContext.Default.JsonElement));
    }

    /// <summary>
    /// Creates a <see cref="Schema"/> from the specified <paramref name="element" />
    /// </summary>
    /// <param name="element">The JSON element to create the schema from.</param>
    /// <returns>A <see cref="Schema"/> instance representing the supplied <param name="element" />.</returns>
    /// <exception cref="JsonException">Thrown if the <paramref name="element" /> cannot be converted to a <see cref="Schema" />.</exception>
    public static Schema FromJsonElement(JsonElement element)
    {
        var typeProperty = element.GetProperty(TypePropertyName);
        bool? isNullable = null;
        string? type = null;

        if (typeProperty.ValueKind == JsonValueKind.String)
        {
            type = typeProperty.GetString();
        }
        else if (typeProperty.ValueKind == JsonValueKind.Array)
        {
            // System.Text.Json outputs a list of types as an array of strings, including `null` when the type is
            // nullable (not necessarily because it is _actually_ marked as such in C#).
            //
            // Gemini API expects a single type (as per OpenAPI spec) but accepts a `nullable` property, so we can do
            // some mapping here.
            foreach (var item in typeProperty.EnumerateArray())
            {
                if (item.GetString() == "null")
                {
                    isNullable = true;
                    continue;
                }

                if (type is not null)
                {
                    throw new JsonException(
                        $"Cannot choose \"best\" '{TypePropertyName}' from options '{type}' and '{item.GetString()}'. Provide a single '{TypePropertyName}'.");
                }

                type = item.GetString() ?? throw new JsonException("Type property must be a string");
            }
        }
        else
        {
            throw new JsonException($"The '{TypePropertyName}' property must be a string or an array of strings");
        }

        if (type is null)
        {
            throw new JsonException($"The '{TypePropertyName}' property must be specified");
        }

        var schemaInfo = SchemaInfo.FromJsonElement(element, isNullable);

        if (string.Equals(type, "STRING", StringComparison.OrdinalIgnoreCase))
        {
            return StringSchema.Create(element, schemaInfo);
        }

        if (string.Equals(type, "NUMBER", StringComparison.OrdinalIgnoreCase))
        {
            return NumberSchema.Create(schemaInfo);
        }

        if (string.Equals(type, "INTEGER", StringComparison.OrdinalIgnoreCase))
        {
            return IntegerSchema.Create(schemaInfo);
        }

        if (string.Equals(type, "BOOLEAN", StringComparison.OrdinalIgnoreCase))
        {
            return BooleanSchema.Create(schemaInfo);
        }

        if (string.Equals(type, "ARRAY", StringComparison.OrdinalIgnoreCase))
        {
            return ArraySchema.Create(element, schemaInfo);
        }

        if (string.Equals(type, "OBJECT", StringComparison.OrdinalIgnoreCase))
        {
            return ObjectSchema.Create(element, schemaInfo);
        }

        throw new JsonException($"Invalid schema {TypePropertyName}: '{type}'");
    }

    internal readonly record struct SchemaInfo
    {
        public string? Format { get; private init; }

        public string? Description { get; private init; }

        public bool? Nullable { get; private init; }

        public static SchemaInfo FromJsonElement(JsonElement element, bool? isNullable = null)
        {
            return new SchemaInfo
            {
                Format = element.TryGetProperty(FormatPropertyName, out var format)
                    ? format.GetString()
                    : null,
                Description = element.TryGetProperty(DescriptionPropertyName, out var description)
                    ? description.GetString()
                    : null,
                Nullable = isNullable
                    ?? (element.TryGetProperty(NullablePropertyName, out var nullable)
                        ? nullable.GetBoolean()
                        : null)
            };
        }
    }
}

internal static class JsonElementExtensions
{
    private const char RootCharacter = '#';
    private const char SeparatorCharacter = '/';
    private const char TildeCharacter = '~';
    private static string EscapedSeparator => $"{TildeCharacter}1";
    private static string EscapedTilde => $"{TildeCharacter}0";
    private static string RootPathStart => $"{RootCharacter}{SeparatorCharacter}";

    public static bool TryGetFromReference(
        this JsonElement element,
        string referencePath,
        out JsonElement? value
    )
    {
        value = null;

        if (
            string.IsNullOrEmpty(referencePath) ||
            !referencePath.StartsWith(RootPathStart, StringComparison.OrdinalIgnoreCase)
        )
        {
            return false;
        }

        var result = true;
        var current = element;
        var segments = referencePath[2..].Split(SeparatorCharacter);

        foreach (var segment in segments)
        {
            var unescapedSegment = segment
                .Replace(EscapedSeparator, SeparatorCharacter.ToString())
                .Replace(EscapedTilde, TildeCharacter.ToString());

            if (current.ValueKind is JsonValueKind.Object)
            {
                if (!current.TryGetProperty(unescapedSegment, out var next))
                {
                    result = false;
                    break;
                }

                current = next;
            }

            if (current.ValueKind is JsonValueKind.Array)
            {
                if (!int.TryParse(unescapedSegment, out var index) || index < 0)
                {
                    result = false;
                    break;
                }

                if (index >= current.GetArrayLength())
                {
                    result = false;
                    break;
                }

                var next = current[index];
                current = next;
            }
        }

        value = current;
        return result;
    }
}
