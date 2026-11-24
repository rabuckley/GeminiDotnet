using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Media;

/// <summary>
/// Backend response for a Diff download response.
/// For details on the Scotty Diff protocol,
/// visit http://go/scotty-diff-protocol.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record DiffDownloadResponse
{
    /// <summary>
    /// The original object location.
    /// </summary>
    [JsonPropertyName("objectLocation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CompositeMedia? ObjectLocation { get; init; }
}

