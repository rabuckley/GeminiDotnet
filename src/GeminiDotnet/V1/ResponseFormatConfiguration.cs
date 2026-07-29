using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Configuration for the response output format. This is a flat object
/// where each optional sub-field configures a specific output modality.
/// </summary>
public sealed record ResponseFormatConfiguration
{
    /// <summary>
    /// Optional. Audio output format configuration.
    /// </summary>
    [JsonPropertyName("audio")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public AudioResponseFormat? Audio { get; init; }

    /// <summary>
    /// Optional. Image output format configuration.
    /// </summary>
    [JsonPropertyName("image")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ImageResponseFormat? Image { get; init; }

    /// <summary>
    /// Optional. Text output format configuration.
    /// </summary>
    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TextResponseFormat? Text { get; init; }
}

