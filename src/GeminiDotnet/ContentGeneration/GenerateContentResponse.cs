using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Response from the model supporting multiple candidate responses.
/// </summary>
/// <remarks>
/// Safety ratings and content filtering are reported for both prompt in <see cref="PromptFeedback"/> and for each
/// <see cref="Candidate"/> in <see cref="Candidate.FinishReason"/> and in <see cref="Candidate.SafetyRatings"/>. The API:
/// <list type="bullet">
/// <item>Returns either all requested candidates or none of them</item>
/// <item>Returns no <see cref="Candidates"/> at all only if there was something wrong with the prompt (check <see cref="PromptFeedback"/>)</item>
/// <item>Reports feedback on each <see cref="Candidate"/> in <see cref="Candidate.FinishReason"/> and in <see cref="Candidate.SafetyRatings"/>.</item>
/// </list>
/// </remarks>
public sealed record GenerateContentResponse
{
    /// <summary>
    /// Candidate responses from the model.
    /// </summary>
    [JsonPropertyName("candidates")]
    public required IReadOnlyList<Candidate> Candidates { get; init; }

    /// <summary>
    /// Returns the prompt's feedback related to the content filters.
    /// </summary>
    [JsonPropertyName("promptFeedback")]
    public PromptFeedback? PromptFeedback { get; init; }

    /// <summary>
    /// Output only. Metadata on the generation requests' token usage.
    /// </summary>
    [JsonPropertyName("usageMetadata")]
    public required UsageMetadata UsageMetadata { get; init; }

    /// <summary>
    /// Output only. The model version used to generate the response.
    /// </summary>
    [JsonPropertyName("modelVersion")]
    public required string ModelVersion { get; init; }

    /// <summary>
    /// Output only. <see cref="ResponseId"/> is used to identify each response.
    /// </summary>
    [JsonPropertyName("responseId")]
    public required string ResponseId { get; init; }
}
