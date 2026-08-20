using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. Configures transcription mode. Supported values: <c>VERBATIM</c>,
/// <c>SMART</c>. If unspecified, defaults to <c>VERBATIM</c> transcription.
/// In <c>SMART</c> mode, the model performs disfluency removal (eliminating
/// filler words, repetitions, and false starts), light grammatical cleanup,
/// automatic formatting (paragraphs, bullet points, numbered lists), and
/// minor user edits (inline self-corrections).
/// Timestamps and diarization are incompatible with mode <c>SMART</c>.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<AudioTranscriptionConfigMode>))]
public enum AudioTranscriptionConfigMode
{
    /// <summary>
    /// Unspecified transcription mode.
    /// </summary>
    [JsonStringEnumMemberName("MODE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Verbatim transcription mode.
    /// </summary>
    [JsonStringEnumMemberName("VERBATIM")]
    Verbatim,

    /// <summary>
    /// Smart transcription mode.
    /// </summary>
    [JsonStringEnumMemberName("SMART")]
    Smart,
}

