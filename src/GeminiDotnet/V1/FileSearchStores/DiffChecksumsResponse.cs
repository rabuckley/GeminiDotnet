using System.Text.Json.Serialization;

namespace GeminiDotnet.V1.FileSearchStores;

/// <summary>
/// Backend response for a Diff get checksums response.
/// For details on the Scotty Diff protocol,
/// visit http://go/scotty-diff-protocol.
/// </summary>
public sealed record DiffChecksumsResponse
{
    /// <summary>
    /// Exactly one of these fields must be populated.
    /// If checksums_location is filled, the server will return the corresponding
    /// contents to the user.  If object_location is filled, the server will
    /// calculate the checksums based on the content there and return that to the
    /// user.
    /// For details on the format of the checksums,
    /// see http://go/scotty-diff-protocol.
    /// </summary>
    [JsonPropertyName("checksumsLocation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CompositeMedia? ChecksumsLocation { get; init; }

    /// <summary>
    /// The chunk size of checksums.  Must be a multiple of 256KB.
    /// </summary>
    [JsonPropertyName("chunkSizeBytes")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? ChunkSizeBytes { get; init; }

    /// <summary>
    /// If set, calculate the checksums based on the contents and return them to
    /// the caller.
    /// </summary>
    [JsonPropertyName("objectLocation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CompositeMedia? ObjectLocation { get; init; }

    /// <summary>
    /// The total size of the server object.
    /// </summary>
    [JsonPropertyName("objectSizeBytes")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? ObjectSizeBytes { get; init; }

    /// <summary>
    /// The object version of the object the checksums are being returned for.
    /// </summary>
    [JsonPropertyName("objectVersion")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? ObjectVersion { get; init; }
}

