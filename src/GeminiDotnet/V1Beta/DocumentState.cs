using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Output only. Current state of the `Document`.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<DocumentState>))]
public enum DocumentState
{
    /// <summary>
    /// The default value. This value is used if the state is omitted.
    /// </summary>
    [JsonStringEnumMemberName("STATE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Some `Chunks` of the `Document` are being processed (embedding and vector
    /// storage).
    /// </summary>
    [JsonStringEnumMemberName("STATE_PENDING")]
    Pending,

    /// <summary>
    /// All `Chunks` of the `Document` is processed and available for querying.
    /// </summary>
    [JsonStringEnumMemberName("STATE_ACTIVE")]
    Active,

    /// <summary>
    /// Some `Chunks` of the `Document` failed processing.
    /// </summary>
    [JsonStringEnumMemberName("STATE_FAILED")]
    Failed,
}

