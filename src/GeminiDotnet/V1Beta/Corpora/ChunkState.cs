using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Corpora;

/// <summary>
/// Output only. Current state of the `Chunk`.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ChunkState>))]
public enum ChunkState
{
    /// <summary>
    /// The default value. This value is used if the state is omitted.
    /// </summary>
    [JsonStringEnumMemberName("STATE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// `Chunk` is being processed (embedding and vector storage).
    /// </summary>
    [JsonStringEnumMemberName("STATE_PENDING_PROCESSING")]
    PendingProcessing,

    /// <summary>
    /// `Chunk` is processed and available for querying.
    /// </summary>
    [JsonStringEnumMemberName("STATE_ACTIVE")]
    Active,

    /// <summary>
    /// `Chunk` failed processing.
    /// </summary>
    [JsonStringEnumMemberName("STATE_FAILED")]
    Failed,
}

