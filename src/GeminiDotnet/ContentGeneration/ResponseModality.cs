using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Supported modalities of the response.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ResponseModality>))]
public enum ResponseModality
{
    /// <summary>
    /// Default value.
    /// </summary>
    [JsonStringEnumMemberName("MODALITY_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Indicates the model should return text.
    /// </summary>
    [JsonStringEnumMemberName("TEXT")]
    Text,

    /// <summary>
    /// Indicates the model should return images.
    /// </summary>
    [JsonStringEnumMemberName("IMAGE")]
    Image,

    /// <summary>
    /// Indicates the model should return audio.
    /// </summary>
    [JsonStringEnumMemberName("AUDIO")]
    Audio,
}
