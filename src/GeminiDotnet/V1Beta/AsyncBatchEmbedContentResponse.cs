using System.Text.Json.Serialization;
using GeminiDotnet.V1Beta.Batches;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Response for a <c>BatchGenerateContent</c> operation.
/// </summary>
public sealed record AsyncBatchEmbedContentResponse
{
    /// <summary>
    /// Output only. The output of the batch request.
    /// </summary>
    [JsonPropertyName("output")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public EmbedContentBatchOutput? Output { get; init; }
}

