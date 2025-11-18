using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Required. The probability of harm for this content.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<SafetyRatingProbability>))]
public enum SafetyRatingProbability
{
    /// <summary>
    /// Probability is unspecified.
    /// </summary>
    [JsonStringEnumMemberName("HARM_PROBABILITY_UNSPECIFIED")]
    HarmProbabilityUnspecified,

    /// <summary>
    /// Content has a negligible chance of being unsafe.
    /// </summary>
    [JsonStringEnumMemberName("NEGLIGIBLE")]
    Negligible,

    /// <summary>
    /// Content has a low chance of being unsafe.
    /// </summary>
    [JsonStringEnumMemberName("LOW")]
    Low,

    /// <summary>
    /// Content has a medium chance of being unsafe.
    /// </summary>
    [JsonStringEnumMemberName("MEDIUM")]
    Medium,

    /// <summary>
    /// Content has a high chance of being unsafe.
    /// </summary>
    [JsonStringEnumMemberName("HIGH")]
    High,
}

