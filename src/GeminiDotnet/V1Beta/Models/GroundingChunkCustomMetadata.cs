using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// User provided metadata about the GroundingFact.
/// </summary>
public sealed record GroundingChunkCustomMetadata
{
    /// <summary>
    /// The key of the metadata.
    /// </summary>
    [JsonPropertyName("key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Key { get; init; }

    /// <summary>
    /// Optional. The numeric value of the metadata.
    /// The expected range for this value depends on the specific <see cref="Key"/> used.
    /// </summary>
    [JsonPropertyName("numericValue")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float? NumericValue { get; init; }

    /// <summary>
    /// Optional. A list of string values for the metadata.
    /// </summary>
    [JsonPropertyName("stringListValue")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public GroundingChunkStringList? StringListValue { get; init; }

    /// <summary>
    /// Optional. The string value of the metadata.
    /// </summary>
    [JsonPropertyName("stringValue")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? StringValue { get; init; }
}

