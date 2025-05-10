using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Content Part modality.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ContentPartModality>))]
public enum ContentPartModality
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
    /// Indicates the model should return audio.
    /// </summary>
    [JsonStringEnumMemberName("AUDIO")]
    Audio,

    /// <summary>
    /// Document.
    /// </summary>
    [JsonStringEnumMemberName("DOCUMENT")]
    Document,
}

