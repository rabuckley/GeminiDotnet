using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Batch request to get a text embedding from the model.
/// </summary>
public sealed record BatchEmbedTextRequest
{
    /// <summary>
    /// Optional. Embed requests for the batch. Only one of <see cref="Texts"/> or <see cref="Requests"/> can be set.
    /// </summary>
    [JsonPropertyName("requests")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<EmbedTextRequest>? Requests { get; init; }

    /// <summary>
    /// Optional. The free-form input texts that the model will turn into an embedding. The
    /// current limit is 100 texts, over which an error will be thrown.
    /// </summary>
    [JsonPropertyName("texts")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<string>? Texts { get; init; }
}

