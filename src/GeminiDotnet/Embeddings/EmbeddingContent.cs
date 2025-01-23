using System.Collections.Generic;
using System.Text.Json.Serialization;

using GeminiDotnet.ContentGeneration;

namespace GeminiDotnet.Embeddings;

public sealed class EmbeddingContent
{
    [JsonPropertyName("parts")]
    public required IEnumerable<ContentPart> Parts { get; init; }
}