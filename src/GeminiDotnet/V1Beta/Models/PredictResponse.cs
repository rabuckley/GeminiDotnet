using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Response message for [PredictionService.Predict].
/// </summary>
public sealed record PredictResponse
{
    /// <summary>
    /// The outputs of the prediction call.
    /// </summary>
    [JsonPropertyName("predictions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<JsonElement> Predictions { get; init; }
}

