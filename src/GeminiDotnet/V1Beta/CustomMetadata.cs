using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// User provided metadata stored as key-value pairs.
/// </summary>
public sealed record CustomMetadata
{
    /// <summary>
    /// Required. The key of the metadata to store.
    /// </summary>
    [JsonPropertyName("key")]
    public required string Key { get; init; }

    /// <summary>
    /// The numeric value of the metadata to store.
    /// </summary>
    [JsonPropertyName("numericValue")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float? NumericValue { get; init; }

    /// <summary>
    /// The StringList value of the metadata to store.
    /// </summary>
    [JsonPropertyName("stringListValue")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public StringList? StringListValue { get; init; }

    /// <summary>
    /// The string value of the metadata to store.
    /// </summary>
    [JsonPropertyName("stringValue")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? StringValue { get; init; }
}

