using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.CachedContents;

/// <summary>
/// The Tool configuration containing parameters for specifying <see cref="V1Beta.CachedContents.Tool"/> use
/// in the request.
/// </summary>
public sealed record ToolConfiguration
{
    /// <summary>
    /// Optional. Function calling config.
    /// </summary>
    [JsonPropertyName("functionCallingConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FunctionCallingConfiguration? FunctionCallingConfiguration { get; init; }

    /// <summary>
    /// Optional. Retrieval config.
    /// </summary>
    [JsonPropertyName("retrievalConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public RetrievalConfiguration? RetrievalConfiguration { get; init; }
}

