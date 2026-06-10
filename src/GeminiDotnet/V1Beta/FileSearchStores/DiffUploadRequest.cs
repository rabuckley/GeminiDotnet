using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// A Diff upload request.
/// For details on the Scotty Diff protocol,
/// visit http://go/scotty-diff-protocol.
/// </summary>
public sealed record DiffUploadRequest
{
    /// <summary>
    /// The location of the checksums for the new object.
    /// Agents must clone the object located here, as the upload server will
    /// delete the contents once a response is received.
    /// For details on the format of the checksums,
    /// see http://go/scotty-diff-protocol.
    /// </summary>
    [JsonPropertyName("checksumsInfo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CompositeMedia? ChecksumsInfo { get; init; }

    /// <summary>
    /// The location of the new object.
    /// Agents must clone the object located here, as the upload server will
    /// delete the contents once a response is received.
    /// </summary>
    [JsonPropertyName("objectInfo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CompositeMedia? ObjectInfo { get; init; }

    /// <summary>
    /// The object version of the object that is the base version the incoming
    /// diff script will be applied to.
    /// This field will always be filled in.
    /// </summary>
    [JsonPropertyName("objectVersion")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? ObjectVersion { get; init; }
}

