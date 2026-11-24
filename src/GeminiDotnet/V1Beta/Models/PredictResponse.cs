using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Response message for [PredictionService.Predict].
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record PredictResponse
{
    /// <summary>
    /// The outputs of the prediction call.
    /// </summary>
    [JsonPropertyName("predictions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<JsonElement> Predictions { get; init; }
}

