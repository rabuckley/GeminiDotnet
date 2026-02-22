using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// Output only. Current state of the <c>Document</c>.
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
    /// Some <c>Chunks</c> of the <c>Document</c> are being processed (embedding and vector
    /// storage).
    /// </summary>
    [JsonStringEnumMemberName("STATE_PENDING")]
    Pending,

    /// <summary>
    /// All <c>Chunks</c> of the <c>Document</c> is processed and available for querying.
    /// </summary>
    [JsonStringEnumMemberName("STATE_ACTIVE")]
    Active,

    /// <summary>
    /// Some <c>Chunks</c> of the <c>Document</c> failed processing.
    /// </summary>
    [JsonStringEnumMemberName("STATE_FAILED")]
    Failed,
}

