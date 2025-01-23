using System.Collections.Generic;
using System.Text.Json.Serialization;

using GeminiDotnet.ContentGeneration.Safety;

namespace GeminiDotnet.ContentGeneration;

public sealed class PromptFeedback
{
    [JsonPropertyName("safetyRatings")]
    public required IEnumerable<SafetyRating> SafetyRatings { get; init; }
}