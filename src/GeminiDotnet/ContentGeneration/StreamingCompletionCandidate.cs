using System.Text.Json.Serialization;

using GeminiDotnet.ContentGeneration.Safety;

namespace GeminiDotnet.ContentGeneration;

public sealed record StreamingCompletionCandidate
{
    [JsonPropertyName("content")]
    public required CandidateContent Content { get; init; }

    [JsonPropertyName("finishReason")]
    public FinishReason? FinishReason { get; init; }

    [JsonPropertyName("safetyRatings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IEnumerable<SafetyRating>? SafetyRatings { get; init; }

    [JsonPropertyName("index")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? Index { get; init; }
}