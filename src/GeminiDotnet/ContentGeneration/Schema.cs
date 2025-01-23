using System.Collections.Generic;
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
    [JsonPropertyName("format")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Format { get; init; }

    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; init; }

    [JsonPropertyName("nullable")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Nullable { get; init; }
}

public sealed record StringSchema : Schema
{
    [JsonPropertyName("enumValues")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<string>? EnumValues { get; init; }
}

public sealed record NumberSchema : Schema;

public sealed record IntegerSchema : Schema;

public sealed record BooleanSchema : Schema;

public sealed record ArraySchema : Schema
{
    [JsonPropertyName("items")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Schema? ItemsSchema { get; init; }
}

public sealed record ObjectSchema : Schema
{
    [JsonPropertyName("properties")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IDictionary<string, Schema>? Properties { get; init; }

    [JsonPropertyName("requiredProperties")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<string>? RequiredProperties { get; init; }
}