using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Google search entry point.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record SearchEntryPoint
{
    /// <summary>
    /// Optional. Web content snippet that can be embedded in a web page or an app webview.
    /// </summary>
    [JsonPropertyName("renderedContent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? RenderedContent { get; init; }

    /// <summary>
    /// Optional. Base64 encoded JSON representing array of  tuple.
    /// </summary>
    [JsonPropertyName("sdkBlob")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<byte> SdkBlob { get; init; }
}

