using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// Configuration for a white space chunking algorithm [white space delimited].
/// </summary>
public sealed record WhiteSpaceConfiguration
{
    /// <summary>
    /// Maximum number of overlapping tokens between two adjacent chunks.
    /// </summary>
    [JsonPropertyName("maxOverlapTokens")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? MaxOverlapTokens { get; init; }

    /// <summary>
    /// Maximum number of tokens per chunk.
    /// Tokens are defined as words for this chunking algorithm.
    /// Note: we are defining tokens as words split by whitespace as opposed to
    /// the output of a tokenizer. The context window of the latest gemini
    /// embedding model as of 2025-04-17 is currently 8192 tokens. We assume that
    /// the average word is 5 characters. Therefore, we set the upper limit to
    /// 2**9, which is 512 words, or 2560 tokens, assuming worst case a
    /// character per token. This is a conservative estimate meant to prevent
    /// context window overflow.
    /// </summary>
    [JsonPropertyName("maxTokensPerChunk")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? MaxTokensPerChunk { get; init; }
}

