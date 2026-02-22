using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Google search entry point.
/// </summary>
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

