using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Status of the url retrieval.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<UrlMetadataUrlRetrievalStatus>))]
public enum UrlMetadataUrlRetrievalStatus
{
    /// <summary>
    /// Default value. This value is unused.
    /// </summary>
    [JsonStringEnumMemberName("URL_RETRIEVAL_STATUS_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Url retrieval is successful.
    /// </summary>
    [JsonStringEnumMemberName("URL_RETRIEVAL_STATUS_SUCCESS")]
    Success,

    /// <summary>
    /// Url retrieval is failed due to error.
    /// </summary>
    [JsonStringEnumMemberName("URL_RETRIEVAL_STATUS_ERROR")]
    Error,

    /// <summary>
    /// Url retrieval is failed because the content is behind paywall.
    /// </summary>
    [JsonStringEnumMemberName("URL_RETRIEVAL_STATUS_PAYWALL")]
    Paywall,

    /// <summary>
    /// Url retrieval is failed because the content is unsafe.
    /// </summary>
    [JsonStringEnumMemberName("URL_RETRIEVAL_STATUS_UNSAFE")]
    Unsafe,
}

