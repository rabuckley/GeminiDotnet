using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// The Tool configuration containing parameters for specifying Tool use in the request.
/// </summary>
public sealed record ToolConfiguration
{
    /// <summary>
    /// Function calling config.
    /// </summary>
    [JsonPropertyName("functionCallingConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FunctionCallingConfiguration? FunctionCallingConfiguration { get; init; }
}