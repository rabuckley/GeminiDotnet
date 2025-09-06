using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Status of the url retrieval.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<UrlRetrievalStatus>))]
public enum UrlRetrievalStatus
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
