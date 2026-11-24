using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Response message for [PredictionService.PredictLongRunning]
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record PredictLongRunningResponse
{
    /// <summary>
    /// The response of the video generation prediction.
    /// </summary>
    [JsonPropertyName("generateVideoResponse")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PredictLongRunningGeneratedVideoResponse? GenerateVideoResponse { get; init; }
}

