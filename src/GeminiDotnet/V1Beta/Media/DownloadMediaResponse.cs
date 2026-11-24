using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Media;

/// <summary>
/// Response for DownloadMedia.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record DownloadMediaResponse
{
    /// <summary>
    /// Output only. The blob data.
    /// </summary>
    [JsonPropertyName("blob")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public GdataMedia? Blob { get; init; }
}

