using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(StringSchema), "STRING")]
[JsonDerivedType(typeof(NumberSchema), "NUMBER")]
[JsonDerivedType(typeof(IntegerSchema), "INTEGER")]
[JsonDerivedType(typeof(BooleanSchema), "BOOLEAN")]
[JsonDerivedType(typeof(ArraySchema), "ARRAY")]
[JsonDerivedType(typeof(ObjectSchema), "OBJECT")]
public abstract record Schema
{
    /// <summary>
    /// The format of the data. This is used only for primitive datatypes.
    /// </summary>
    [JsonPropertyName("format")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Format { get; init; }

    /// <summary>
    /// A brief description of the parameter. This could contain examples of use. Parameter description may be
    /// formatted as Markdown.
    /// </summary>
    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; init; }

    /// <summary>
    /// Indicates if the value may be null.
    /// </summary>
    [JsonPropertyName("nullable")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Nullable { get; init; }
}

public sealed record StringSchema : Schema
{
    /// <summary>
    /// Possible values of the element of type <c>STRING</c> with enum format.
    /// </summary>
    [JsonPropertyName("enumValues")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<string>? EnumValues { get; init; }
}

public sealed record NumberSchema : Schema;

public sealed record IntegerSchema : Schema;

public sealed record BooleanSchema : Schema;

public sealed record ArraySchema : Schema
{
    /// <summary>
    /// Maximum number of the elements
    /// </summary>
    [JsonPropertyName("maxItems")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxItems { get; init; }

    /// <summary>
    /// Minimum number of the elements
    /// </summary>
    [JsonPropertyName("minItems")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MinItems { get; init; }

    /// <summary>
    /// Schema of the elements.
    /// </summary>
    [JsonPropertyName("items")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Schema? Items { get; init; }
}

public sealed record ObjectSchema : Schema
{
    /// <summary>
    /// Properties of the object.
    /// </summary>
    [JsonPropertyName("properties")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IDictionary<string, Schema>? Properties { get; init; }

    /// <summary>
    /// Required properties of the object.
    /// </summary>
    [JsonPropertyName("required")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<string>? RequiredProperties { get; init; }
}