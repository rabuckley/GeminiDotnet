using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// Response for DownloadMedia.
/// </summary>
public sealed record DownloadMediaResponse
{
    /// <summary>
    /// Output only. The blob data.
    /// </summary>
    [JsonPropertyName("blob")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public GdataMedia? Blob { get; init; }
}

