using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Request message for PredictionService.Predict.
/// </summary>
public sealed record PredictRequest
{
    /// <summary>
    /// Required. The instances that are the input to the prediction call.
    /// </summary>
    [JsonPropertyName("instances")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<JsonElement> Instances { get; init; }

    /// <summary>
    /// Optional. Labels with user-defined metadata for the request.
    /// Optional. Labels must follow standard unified Cloud label requirements:
    /// - Label keys must start with a letter.
    /// - Label keys and values can be no longer than 63 characters (Unicode
    /// codepoints) and can only contain lowercase letters, numeric characters,
    /// underscores, and dashes.
    /// - International characters are allowed.
    /// Usage:
    /// -  Safety identifiers from aggregators: Use the key <c>safety_identifier</c>
    /// (e.g. <c>{"safety_identifier": "user_session_123"}</c>)
    /// </summary>
    [JsonPropertyName("labels")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyDictionary<string, string>? Labels { get; init; }

    /// <summary>
    /// Optional. The parameters that govern the prediction call.
    /// </summary>
    [JsonPropertyName("parameters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JsonElement Parameters { get; init; }
}

