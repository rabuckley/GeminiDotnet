using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.CachedContents;

/// <summary>
/// Different types of search that can be enabled on the GoogleSearch tool.
/// </summary>
public sealed record SearchTypes
{
    /// <summary>
    /// Optional. Setting this field enables image search. Image bytes are returned.
    /// </summary>
    [JsonPropertyName("imageSearch")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ImageSearch? ImageSearch { get; init; }

    /// <summary>
    /// Optional. Setting this field enables web search. Only text results are returned.
    /// </summary>
    [JsonPropertyName("webSearch")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public WebSearch? WebSearch { get; init; }
}

