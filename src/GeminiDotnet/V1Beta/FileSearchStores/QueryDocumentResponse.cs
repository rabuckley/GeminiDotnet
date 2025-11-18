using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// Response from <c>QueryDocument</c> containing a list of relevant chunks.
/// </summary>
public sealed record QueryDocumentResponse
{
    /// <summary>
    /// The returned relevant chunks.
    /// </summary>
    [JsonPropertyName("relevantChunks")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<RelevantChunk>? RelevantChunks { get; init; }
}

