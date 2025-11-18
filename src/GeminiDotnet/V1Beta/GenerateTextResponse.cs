using System.Text.Json.Serialization;
using GeminiDotnet.V1Beta.Models;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// The response from the model, including candidate completions.
/// </summary>
public sealed record GenerateTextResponse
{
    /// <summary>
    /// Candidate responses from the model.
    /// </summary>
    [JsonPropertyName("candidates")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<TextCompletion>? Candidates { get; init; }

    /// <summary>
    /// A set of content filtering metadata for the prompt and response
    /// text.
    /// This indicates which <c>SafetyCategory</c>(s) blocked a
    /// candidate from this response, the lowest <c>HarmProbability</c>
    /// that triggered a block, and the HarmThreshold setting for that category.
    /// This indicates the smallest change to the <c>SafetySettings</c> that would be
    /// necessary to unblock at least 1 response.
    /// The blocking is configured by the <c>SafetySettings</c> in the request (or the
    /// default <c>SafetySettings</c> of the API).
    /// </summary>
    [JsonPropertyName("filters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<ContentFilter>? Filters { get; init; }

    /// <summary>
    /// Returns any safety feedback related to content filtering.
    /// </summary>
    [JsonPropertyName("safetyFeedback")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<SafetyFeedback>? SafetyFeedback { get; init; }
}

