using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Google search entry point.
/// </summary>
public sealed record SearchEntryPoint
{
    /// <summary>
    /// Web content snippet that can be embedded in a web page or an app webview.
    /// </summary>
    [JsonPropertyName("renderedContent")]
    public string? RenderedContent { get; init; }

    /// <summary>
    /// Base64 encoded JSON representing array of <c>&lt;search term, search url&gt;</c> tuple.
    /// A base64-encoded string.
    /// </summary>
    [JsonPropertyName("sdkBlob")]
    public string? SdkBlob { get; init; }
}
