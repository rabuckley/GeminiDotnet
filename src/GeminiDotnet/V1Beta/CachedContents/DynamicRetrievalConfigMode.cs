using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.CachedContents;

/// <summary>
/// The mode of the predictor to be used in dynamic retrieval.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<DynamicRetrievalConfigMode>))]
public enum DynamicRetrievalConfigMode
{
    /// <summary>
    /// Always trigger retrieval.
    /// </summary>
    [JsonStringEnumMemberName("MODE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Run retrieval only when system decides it is necessary.
    /// </summary>
    [JsonStringEnumMemberName("MODE_DYNAMIC")]
    Dynamic,
}

