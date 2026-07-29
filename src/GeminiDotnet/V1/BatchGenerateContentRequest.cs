using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

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

