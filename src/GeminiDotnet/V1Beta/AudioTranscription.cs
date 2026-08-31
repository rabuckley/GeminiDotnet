using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// The transcription of an audio part.
/// For multi-speaker audio, each speaker segment is a separate Part with its
/// own AudioTranscription carrying the speaker_label.
/// </summary>
public sealed record AudioTranscription
{
    /// <summary>
    /// Optional. A label identifying the speaker of this audio segment (e.g. "spk_1",
    /// "spk_2"). Present when diarization is set.
    /// </summary>
    [JsonPropertyName("speakerLabel")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? SpeakerLabel { get; init; }

    /// <summary>
    /// Required. The transcription text of this audio segment.
    /// </summary>
    [JsonPropertyName("text")]
    public required string Text { get; init; }

    /// <summary>
    /// Optional. Detailed word-level transcriptions and timing details.
    /// Present when word_timestamp is set.
    /// </summary>
    [JsonPropertyName("words")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<WordInfo>? Words { get; init; }
}

