using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// A list of floats representing an embedding.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record ContentEmbedding
{
    /// <summary>
    /// This field stores the soft tokens tensor frame shape
    /// (e.g. [1, 1, 256, 2048]).
    /// </summary>
    [JsonPropertyName("shape")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<int> Shape { get; init; }

    /// <summary>
    /// The embedding values. This is for 3P users only and will not be populated
    /// for 1P calls.
    /// </summary>
    [JsonPropertyName("values")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<float> Values { get; init; }
}

