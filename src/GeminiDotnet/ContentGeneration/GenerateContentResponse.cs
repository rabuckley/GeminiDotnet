using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

public sealed record GenerateContentResponse
{
    [JsonPropertyName("candidates")]
    public required IEnumerable<CandidateResponse> Candidates { get; init; }

    [JsonPropertyName("usageMetadata")]
    public required UsageMetadata UsageMetadata { get; init; }

    [JsonPropertyName("promptFeedback")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PromptFeedback? PromptFeedback { get; init; }

    [JsonPropertyName("modelVersion")]
    public required string ModelVersion { get; set; }
}