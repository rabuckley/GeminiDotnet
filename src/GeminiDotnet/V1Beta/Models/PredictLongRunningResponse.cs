using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Response message for [PredictionService.PredictLongRunning]
/// </summary>
public sealed record PredictLongRunningResponse
{
    /// <summary>
    /// The response of the video generation prediction.
    /// </summary>
    [JsonPropertyName("generateVideoResponse")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PredictLongRunningGeneratedVideoResponse? GenerateVideoResponse { get; init; }
}

