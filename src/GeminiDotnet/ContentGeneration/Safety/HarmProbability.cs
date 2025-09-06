using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.Safety;

/// <summary>
/// The probability that a piece of content is harmful. The classification system gives the probability of the content
/// being unsafe. This does not indicate the severity of harm for a piece of content.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<HarmProbability>))]
public enum HarmProbability
{
    [JsonPropertyName("HARM_PROBABILITY_UNSPECIFIED")]
    Unspecified,

    [JsonPropertyName("NEGLIGIBLE")]
    Negligible,

    [JsonPropertyName("LOW")]
    Low,

    [JsonPropertyName("MEDIUM")]
    Medium,

    [JsonPropertyName("HIGH")]
    High,
}
