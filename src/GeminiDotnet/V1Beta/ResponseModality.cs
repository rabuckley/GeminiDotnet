using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. The requested modalities of the response. Represents the set of modalities
/// that the model can return, and should be expected in the response. This is
/// an exact match to the modalities of the response.
/// A model may have multiple combinations of supported modalities. If the
/// requested modalities do not match any of the supported combinations, an
/// error will be returned.
/// An empty list is equivalent to requesting only text.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ResponseModality>))]
public enum ResponseModality
{
    [JsonStringEnumMemberName("MODALITY_UNSPECIFIED")]
    Unspecified,

    [JsonStringEnumMemberName("TEXT")]
    Text,

    [JsonStringEnumMemberName("IMAGE")]
    Image,

    [JsonStringEnumMemberName("AUDIO")]
    Audio,
}

