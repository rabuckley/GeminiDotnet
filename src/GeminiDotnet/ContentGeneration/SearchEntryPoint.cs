using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Google search entry point.
/// </summary>
public sealed record SearchEntryPoint
{
    [JsonPropertyName("renderedContent")]
    public required string RenderedContent { get; init; }

    /// <summary>
    /// Base64 encoded JSON representing array of <c>&lt;search term, search url&gt;</c> tuple.
    /// A base64-encoded string.
    /// </summary>
    [JsonPropertyName("sdkBlob")]
    public required string SdkBlob { get; init; }
}
