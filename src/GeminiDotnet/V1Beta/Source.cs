using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// A source to be mounted into the environment.
/// </summary>
public sealed record Source
{
    /// <summary>
    /// The inline content if <see cref="Type"/> is <c>INLINE</c>.
    /// </summary>
    [JsonPropertyName("content")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Content { get; init; }

    /// <summary>
    /// Optional encoding for inline content (e.g. <c>base64</c>).
    /// </summary>
    [JsonPropertyName("encoding")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Encoding { get; init; }

    /// <summary>
    /// The source of the environment.
    /// For Cloud Storage, this is the Cloud Storage path.
    /// For GitHub, this is the GitHub path.
    /// </summary>
    [JsonPropertyName("source")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? SourceValue { get; init; }

    /// <summary>
    /// Where the source should appear in the environment.
    /// </summary>
    [JsonPropertyName("target")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Target { get; init; }

    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SourceType? Type { get; init; }
}

