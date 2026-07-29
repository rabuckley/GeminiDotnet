using System.Text.Json.Serialization;

namespace GeminiDotnet.V1.FileSearchStores;

/// <summary>
/// Backend response for a Diff get version response.
/// For details on the Scotty Diff protocol,
/// visit http://go/scotty-diff-protocol.
/// </summary>
public sealed record DiffVersionResponse
{
    /// <summary>
    /// The total size of the server object.
    /// </summary>
    [JsonPropertyName("objectSizeBytes")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? ObjectSizeBytes { get; init; }

    /// <summary>
    /// The version of the object stored at the server.
    /// </summary>
    [JsonPropertyName("objectVersion")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? ObjectVersion { get; init; }
}

