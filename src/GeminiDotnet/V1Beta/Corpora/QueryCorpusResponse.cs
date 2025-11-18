using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Corpora;

/// <summary>
/// Response from <c>QueryCorpus</c> containing a list of relevant chunks.
/// </summary>
public sealed record QueryCorpusResponse
{
    /// <summary>
    /// The relevant chunks.
    /// </summary>
    [JsonPropertyName("relevantChunks")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<RelevantChunk>? RelevantChunks { get; init; }
}

