using GeminiDotnet.ContentGeneration.Safety;
using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

public sealed record CandidateResponse
{
    [JsonPropertyName("content")]
    public required Content Content { get; init; }

    [JsonPropertyName("finishReason")]
    public FinishReason? FinishReason { get; init; }

    [JsonPropertyName("avgLogprobs")]
    public float? AvgLogProbs { get; init; }

    [JsonPropertyName("index")]
    public int? Index { get; init; }

    [JsonPropertyName("safetyRatings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<SafetyRating>? SafetyRatings { get; init; }
}