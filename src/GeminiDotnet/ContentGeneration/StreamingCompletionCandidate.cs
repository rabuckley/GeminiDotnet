using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

public sealed record StreamingCompletionCandidate
{
    [JsonPropertyName("content")]
    public required CandidateContent Content { get; init; }

    [JsonPropertyName("finishReason")]
    public FinishReason? FinishReason { get; init; }
}