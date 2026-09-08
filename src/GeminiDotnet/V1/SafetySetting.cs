using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Safety setting, affecting the safety-blocking behavior.
/// Passing a safety setting for a category changes the allowed probability that
/// content is blocked.
/// </summary>
public sealed record SafetySetting
{
    /// <summary>
    /// Required. The category for this setting.
    /// </summary>
    [JsonPropertyName("category")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public HarmCategory Category { get; init; }

    /// <summary>
    /// Required. Controls the probability threshold at which harm is blocked.
    /// </summary>
    [JsonPropertyName("threshold")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SafetySettingThreshold Threshold { get; init; }
}

