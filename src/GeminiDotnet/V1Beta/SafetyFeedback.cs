using System.Text.Json.Serialization;
using GeminiDotnet.V1Beta.Models;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Safety feedback for an entire request.
/// This field is populated if content in the input and/or response is blocked
/// due to safety settings. SafetyFeedback may not exist for every HarmCategory.
/// Each SafetyFeedback will return the safety settings used by the request as
/// well as the lowest HarmProbability that should be allowed in order to return
/// a result.
/// </summary>
public sealed record SafetyFeedback
{
    /// <summary>
    /// Safety rating evaluated from content.
    /// </summary>
    [JsonPropertyName("rating")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SafetyRating? Rating { get; init; }

    /// <summary>
    /// Safety settings applied to the request.
    /// </summary>
    [JsonPropertyName("setting")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SafetySetting? Setting { get; init; }
}

