using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

public sealed record StreamingTextGenerationResponse
{
    [JsonPropertyName("candidates")]
    public required IEnumerable<StreamingCompletionCandidate> Candidates { get; init; }

    [JsonPropertyName("usageMetadata")]
    public required UsageMetadata UsageMetadata { get; init; }

    [JsonPropertyName("modelVersion")]
    public required string ModelVersion { get; set; }
}