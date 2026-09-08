using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FilesRegister;

/// <summary>
/// Request for <c>RegisterFiles</c>.
/// </summary>
public sealed record RegisterFilesRequest
{
    /// <summary>
    /// Required. The Google Cloud Storage URIs to register. Example: <c>gs://bucket/object</c>.
    /// </summary>
    [JsonPropertyName("uris")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<string>? Uris { get; init; }
}

