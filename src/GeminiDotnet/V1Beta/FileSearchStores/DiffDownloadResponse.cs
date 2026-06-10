using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// Backend response for a Diff download response.
/// For details on the Scotty Diff protocol,
/// visit http://go/scotty-diff-protocol.
/// </summary>
public sealed record DiffDownloadResponse
{
    /// <summary>
    /// The original object location.
    /// </summary>
    [JsonPropertyName("objectLocation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CompositeMedia? ObjectLocation { get; init; }
}

