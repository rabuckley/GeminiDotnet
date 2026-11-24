using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// The request to be processed in the batch.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record InlinedEmbedContentRequest
{
    /// <summary>
    /// Optional. The metadata to be associated with the request.
    /// </summary>
    [JsonPropertyName("metadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JsonElement Metadata { get; init; }

    /// <summary>
    /// Required. The request to be processed in the batch.
    /// </summary>
    [JsonPropertyName("request")]
    public required EmbedContentRequest Request { get; init; }
}

