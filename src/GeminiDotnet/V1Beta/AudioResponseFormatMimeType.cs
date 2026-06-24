using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. The MIME type of the audio output.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<AudioResponseFormatMimeType>))]
public enum AudioResponseFormatMimeType
{
    /// <summary>
    /// Default value. This value is unused.
    /// </summary>
    [JsonStringEnumMemberName("MIME_TYPE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// MP3 audio format.
    /// </summary>
    [JsonStringEnumMemberName("AUDIO_MP3")]
    AudioMp3,

    /// <summary>
    /// OGG Opus audio format.
    /// </summary>
    [JsonStringEnumMemberName("AUDIO_OGG_OPUS")]
    AudioOggOpus,

    /// <summary>
    /// Raw PCM (L16) audio format.
    /// </summary>
    [JsonStringEnumMemberName("AUDIO_L16")]
    AudioL16,

    /// <summary>
    /// WAV audio format.
    /// </summary>
    [JsonStringEnumMemberName("AUDIO_WAV")]
    AudioWav,

    /// <summary>
    /// A-law audio format.
    /// </summary>
    [JsonStringEnumMemberName("AUDIO_ALAW")]
    AudioAlaw,

    /// <summary>
    /// Mu-law audio format.
    /// </summary>
    [JsonStringEnumMemberName("AUDIO_MULAW")]
    AudioMulaw,
}

