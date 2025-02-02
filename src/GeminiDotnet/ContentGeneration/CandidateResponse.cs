using System.Collections.Generic;
using System.Text.Json.Serialization;

using GeminiDotnet.ContentGeneration.Safety;

namespace GeminiDotnet.ContentGeneration;

public sealed record CandidateResponse
{
    [JsonPropertyName("content")]
    public required CandidateContent Content { get; init; }

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