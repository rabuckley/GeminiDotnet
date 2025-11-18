using System.Text.Json.Serialization;
using GeminiDotnet.V1Beta.Batches;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Request for a <c>BatchGenerateContent</c> operation.
/// </summary>
public sealed record BatchGenerateContentRequest
{
    /// <summary>
    /// Required. The batch to create.
    /// </summary>
    [JsonPropertyName("batch")]
    public required GenerateContentBatch Batch { get; init; }
}

