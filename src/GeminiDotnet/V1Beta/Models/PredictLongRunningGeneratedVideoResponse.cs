using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Veo response.
/// </summary>
public sealed record PredictLongRunningGeneratedVideoResponse
{
    /// <summary>
    /// The generated samples.
    /// </summary>
    [JsonPropertyName("generatedSamples")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Media>? GeneratedSamples { get; init; }

    /// <summary>
    /// Returns if any videos were filtered due to RAI policies.
    /// </summary>
    [JsonPropertyName("raiMediaFilteredCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? RaiMediaFilteredCount { get; init; }

    /// <summary>
    /// Returns rai failure reasons if any.
    /// </summary>
    [JsonPropertyName("raiMediaFilteredReasons")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<string>? RaiMediaFilteredReasons { get; init; }
}

