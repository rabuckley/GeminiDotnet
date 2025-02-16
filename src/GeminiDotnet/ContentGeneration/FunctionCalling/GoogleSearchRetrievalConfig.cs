using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.FunctionCalling;

/// <summary>
/// Describes the options to customize dynamic retrieval.
/// </summary>
public sealed record GoogleSearchRetrievalConfig
{
    /// <summary>
    /// The mode of the predictor to be used in dynamic retrieval.
    /// </summary>
    [JsonPropertyName("mode")]
    public required PredictorMode Mode { get; init; }

    /// <summary>
    /// The threshold to be used in dynamic retrieval. If not set, a system default value is used.
    /// </summary>
    [JsonPropertyName("dynamicThreshold")]
    public required double DynamicThreshold { get; init; }
}
