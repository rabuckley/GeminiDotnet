using System.Text.Json.Serialization;

namespace GeminiDotnet.V1.Models;

[JsonConverter(typeof(JsonStringEnumConverter<GenerativeLanguageModality>))]
public enum GenerativeLanguageModality
{
    /// <summary>
    /// Unspecified modality.
    /// </summary>
    [JsonStringEnumMemberName("MODALITY_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Plain text.
    /// </summary>
    [JsonStringEnumMemberName("TEXT")]
    Text,

    /// <summary>
    /// Image.
    /// </summary>
    [JsonStringEnumMemberName("IMAGE")]
    Image,

    /// <summary>
    /// Video.
    /// </summary>
    [JsonStringEnumMemberName("VIDEO")]
    Video,

    /// <summary>
    /// Audio.
    /// </summary>
    [JsonStringEnumMemberName("AUDIO")]
    Audio,

    /// <summary>
    /// Document, e.g. PDF.
    /// </summary>
    [JsonStringEnumMemberName("DOCUMENT")]
    Document,
}

