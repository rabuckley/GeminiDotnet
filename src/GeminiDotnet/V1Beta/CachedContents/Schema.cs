using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.CachedContents;

/// <summary>
/// The <see cref="V1Beta.CachedContents.Schema"/> object allows the definition of input and output data types.
/// These types can be objects, but also primitives and arrays.
/// Represents a select subset of an [OpenAPI 3.0 schema
/// object](https://spec.openapis.org/oas/v3.0.3#schema).
/// </summary>
public sealed record Schema
{
    /// <summary>
    /// Optional. The value should be validated against any (one or more) of the subschemas
    /// in the list.
    /// </summary>
    [JsonPropertyName("anyOf")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Schema>? AnyOf { get; init; }

    /// <summary>
    /// Optional. Default value of the field. Per JSON Schema, this field is intended for
    /// documentation generators and doesn't affect validation. Thus it's included
    /// here and ignored so that developers who send schemas with a <see cref="Default"/> field
    /// don't get unknown-field errors.
    /// </summary>
    [JsonPropertyName("default")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JsonElement Default { get; init; }

    /// <summary>
    /// Optional. A brief description of the parameter. This could contain examples of use.
    /// Parameter description may be formatted as Markdown.
    /// </summary>
    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Description { get; init; }

    /// <summary>
    /// Optional. Possible values of the element of Type.STRING with enum format.
    /// For example we can define an Enum Direction as :
    /// {type:STRING, format:enum, enum:["EAST", NORTH", "SOUTH", "WEST"]}
    /// </summary>
    [JsonPropertyName("enum")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<string>? Enum { get; init; }

    /// <summary>
    /// Optional. Example of the object. Will only populated when the object is the root.
    /// </summary>
    [JsonPropertyName("example")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JsonElement Example { get; init; }

    /// <summary>
    /// Optional. The format of the data. Any value is allowed, but most do not trigger any
    /// special functionality.
    /// </summary>
    [JsonPropertyName("format")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Format { get; init; }

    /// <summary>
    /// Optional. Schema of the elements of Type.ARRAY.
    /// </summary>
    [JsonPropertyName("items")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Schema? Items { get; init; }

    /// <summary>
    /// Optional. Maximum value of the Type.INTEGER and Type.NUMBER
    /// </summary>
    [JsonPropertyName("maximum")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public double? Maximum { get; init; }

    /// <summary>
    /// Optional. Maximum number of the elements for Type.ARRAY.
    /// </summary>
    [JsonPropertyName("maxItems")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? MaxItems { get; init; }

    /// <summary>
    /// Optional. Maximum length of the Type.STRING
    /// </summary>
    [JsonPropertyName("maxLength")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? MaxLength { get; init; }

    /// <summary>
    /// Optional. Maximum number of the properties for Type.OBJECT.
    /// </summary>
    [JsonPropertyName("maxProperties")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? MaxProperties { get; init; }

    /// <summary>
    /// Optional. SCHEMA FIELDS FOR TYPE INTEGER and NUMBER
    /// Minimum value of the Type.INTEGER and Type.NUMBER
    /// </summary>
    [JsonPropertyName("minimum")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public double? Minimum { get; init; }

    /// <summary>
    /// Optional. Minimum number of the elements for Type.ARRAY.
    /// </summary>
    [JsonPropertyName("minItems")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? MinItems { get; init; }

    /// <summary>
    /// Optional. SCHEMA FIELDS FOR TYPE STRING
    /// Minimum length of the Type.STRING
    /// </summary>
    [JsonPropertyName("minLength")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? MinLength { get; init; }

    /// <summary>
    /// Optional. Minimum number of the properties for Type.OBJECT.
    /// </summary>
    [JsonPropertyName("minProperties")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? MinProperties { get; init; }

    /// <summary>
    /// Optional. Indicates if the value may be null.
    /// </summary>
    [JsonPropertyName("nullable")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? Nullable { get; init; }

    /// <summary>
    /// Optional. Pattern of the Type.STRING to restrict a string to a regular expression.
    /// </summary>
    [JsonPropertyName("pattern")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Pattern { get; init; }

    /// <summary>
    /// Optional. Properties of Type.OBJECT.
    /// </summary>
    [JsonPropertyName("properties")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JsonElement Properties { get; init; }

    /// <summary>
    /// Optional. The order of the properties.
    /// Not a standard field in open api spec. Used to determine the order of the
    /// properties in the response.
    /// </summary>
    [JsonPropertyName("propertyOrdering")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<string>? PropertyOrdering { get; init; }

    /// <summary>
    /// Optional. Required properties of Type.OBJECT.
    /// </summary>
    [JsonPropertyName("required")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<string>? Required { get; init; }

    /// <summary>
    /// Optional. The title of the schema.
    /// </summary>
    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Title { get; init; }

    /// <summary>
    /// Required. Data type.
    /// </summary>
    [JsonPropertyName("type")]
    public required Type Type { get; init; }
}

