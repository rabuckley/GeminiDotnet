using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.AuthTokens;

/// <summary>
/// Config for proactivity features.
/// </summary>
public sealed record ProactivityConfiguration
{
    /// <summary>
    /// Optional. If enabled, the model can reject responding to the last prompt. For
    /// example, this allows the model to ignore out of context speech or to stay
    /// silent if the user did not make a request, yet.
    /// </summary>
    [JsonPropertyName("proactiveAudio")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? ProactiveAudio { get; init; }

    /// <summary>
    /// Optional. If enabled, the system might trigger a generation on each received image.
    /// This requires that <c>proactive_audio</c> is enabled. The rate of
    /// generation depends on the rate of incoming images and can cause a
    /// significant increase in cost.
    /// </summary>
    [JsonPropertyName("proactiveVideo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? ProactiveVideo { get; init; }
}

