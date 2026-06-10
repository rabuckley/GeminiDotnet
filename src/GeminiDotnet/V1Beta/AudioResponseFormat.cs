using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Configuration for audio output format.
/// </summary>
public sealed record AudioResponseFormat
{
    /// <summary>
    /// Optional. Bit rate in bits per second (bps). Only applicable for compressed formats
    /// (MP3, Opus).
    /// </summary>
    [JsonPropertyName("bitRate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? BitRate { get; init; }

    /// <summary>
    /// Optional. The delivery mode for the audio output.
    /// </summary>
    [JsonPropertyName("delivery")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public AudioResponseFormatDelivery? Delivery { get; init; }

    /// <summary>
    /// Optional. The MIME type of the audio output.
    /// </summary>
    [JsonPropertyName("mimeType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public AudioResponseFormatMimeType? MimeType { get; init; }

    /// <summary>
    /// Optional. Sample rate in Hz.
    /// </summary>
    [JsonPropertyName("sampleRate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? SampleRate { get; init; }
}

