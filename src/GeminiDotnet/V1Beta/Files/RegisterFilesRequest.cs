using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Files;

/// <summary>
/// Request for <c>RegisterFiles</c>.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record RegisterFilesRequest
{
    /// <summary>
    /// Required. The Google Cloud Storage URIs to register. Example: <c>gs://bucket/object</c>.
    /// </summary>
    [JsonPropertyName("uris")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<string>? Uris { get; init; }
}

