using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.CachedContents;

/// <summary>
/// Describes the options to customize dynamic retrieval.
/// </summary>
public sealed record DynamicRetrievalConfiguration
{
    /// <summary>
    /// The threshold to be used in dynamic retrieval.
    /// If not set, a system default value is used.
    /// </summary>
    [JsonPropertyName("dynamicThreshold")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float? DynamicThreshold { get; init; }

    /// <summary>
    /// The mode of the predictor to be used in dynamic retrieval.
    /// </summary>
    [JsonPropertyName("mode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DynamicRetrievalConfigMode? Mode { get; init; }
}

