using System.Text.Json.Serialization;
using GeminiDotnet.V1Beta.Models;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Response from the model supporting multiple candidate responses.
/// Safety ratings and content filtering are reported for both
/// prompt in <c>GenerateContentResponse.prompt_feedback</c> and for each candidate
/// in <c>finish_reason</c> and in <c>safety_ratings</c>. The API:
/// - Returns either all requested candidates or none of them
/// - Returns no candidates at all only if there was something wrong with the
/// prompt (check <c>prompt_feedback</c>)
/// - Reports feedback on each candidate in <c>finish_reason</c> and
/// <c>safety_ratings</c>.
/// </summary>
public sealed record GenerateContentResponse
{
    /// <summary>
    /// Candidate responses from the model.
    /// </summary>
    [JsonPropertyName("candidates")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Candidate>? Candidates { get; init; }

    /// <summary>
    /// Output only. The current model status of this model.
    /// </summary>
    [JsonPropertyName("modelStatus")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ModelStatus? ModelStatus { get; init; }

    /// <summary>
    /// Output only. The model version used to generate the response.
    /// </summary>
    [JsonPropertyName("modelVersion")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? ModelVersion { get; init; }

    /// <summary>
    /// Returns the prompt's feedback related to the content filters.
    /// </summary>
    [JsonPropertyName("promptFeedback")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PromptFeedback? PromptFeedback { get; init; }

    /// <summary>
    /// Output only. response_id is used to identify each response.
    /// </summary>
    [JsonPropertyName("responseId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? ResponseId { get; init; }

    /// <summary>
    /// Output only. Metadata on the generation requests' token usage.
    /// </summary>
    [JsonPropertyName("usageMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public UsageMetadata? UsageMetadata { get; init; }
}

