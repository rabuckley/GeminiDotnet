using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Configuration for specifying function calling behavior.
/// </summary>
public sealed record FunctionCallingConfiguration
{
    /// <summary>
    /// Specifies the mode in which function calling should execute. If unspecified, the default value will be set to AUTO.
    /// </summary>
    [JsonPropertyName("mode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FunctionCallingMode? Mode { get; init; }

    /// <summary>
    /// A set of function names that, when provided, limits the functions the model will call.
    /// This should only be set when the Mode is ANY. Function names should match <c>[FunctionDeclaration.name]</c>.
    /// With mode set to ANY, model will predict a function call from the set of function names provided.
    /// </summary>
    [JsonPropertyName("allowedFunctionNames")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<string>? AllowedFunctionNames { get; init; }
}
