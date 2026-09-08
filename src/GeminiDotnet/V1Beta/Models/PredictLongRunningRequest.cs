using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Request message for [PredictionService.PredictLongRunning].
/// </summary>
public sealed record PredictLongRunningRequest
{
    /// <summary>
    /// Required. The instances that are the input to the prediction call.
    /// </summary>
    [JsonPropertyName("instances")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<JsonElement> Instances { get; init; }

    /// <summary>
    /// Optional. The parameters that govern the prediction call.
    /// </summary>
    [JsonPropertyName("parameters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JsonElement Parameters { get; init; }
}

