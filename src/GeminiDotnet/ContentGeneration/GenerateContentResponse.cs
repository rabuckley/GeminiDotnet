using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Response from the model supporting multiple candidate responses.
/// </summary>
public sealed record GenerateContentResponse
{
    /// <summary>
    /// Candidate responses from the model.
    /// </summary>
    [JsonPropertyName("candidates")]
    public required IReadOnlyCollection<CandidateResponse> Candidates { get; init; }

    /// <summary>
    /// Returns the prompt's feedback related to the content filters.
    /// </summary>
    [JsonPropertyName("promptFeedback")]
    public PromptFeedback? PromptFeedback { get; init; }

    /// <summary>
    /// Metadata on the generation requests' token usage.
    /// </summary>
    [JsonPropertyName("usageMetadata")]
    public required UsageMetadata UsageMetadata { get; init; }

    /// <summary>
    /// The model version used to generate the response.
    /// </summary>
    [JsonPropertyName("modelVersion")]
    public required string ModelVersion { get; set; }
}