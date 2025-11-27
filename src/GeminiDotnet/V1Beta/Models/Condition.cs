using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Filter condition applicable to a single key.
/// </summary>
public sealed record Condition
{
    /// <summary>
    /// The numeric value to filter the metadata on.
    /// </summary>
    [JsonPropertyName("numericValue")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float? NumericValue { get; init; }

    /// <summary>
    /// Required. Operator applied to the given key-value pair to trigger the condition.
    /// </summary>
    [JsonPropertyName("operation")]
    public required ConditionOperation Operation { get; init; }

    /// <summary>
    /// The string value to filter the metadata on.
    /// </summary>
    [JsonPropertyName("stringValue")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? StringValue { get; init; }
}

