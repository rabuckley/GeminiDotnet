using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

[JsonConverter(typeof(JsonStringEnumConverter<SourceType>))]
public enum SourceType
{
    [JsonStringEnumMemberName("TYPE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// A Cloud Storage bucket.
    /// </summary>
    [JsonStringEnumMemberName("GCS")]
    Gcs,

    /// <summary>
    /// Inline content.
    /// </summary>
    [JsonStringEnumMemberName("INLINE")]
    Inline,

    /// <summary>
    /// A generic repository. The protocol prefix in the source URL
    /// identifies the provider (e.g., github://, gcs://).
    /// </summary>
    [JsonStringEnumMemberName("REPOSITORY")]
    Repository,
}

