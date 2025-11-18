using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.CachedContents;

/// <summary>
/// Configuration for specifying function calling behavior.
/// </summary>
public sealed record FunctionCallingConfiguration
{
    /// <summary>
    /// Optional. A set of function names that, when provided, limits the functions the model
    /// will call.
    /// This should only be set when the Mode is ANY or VALIDATED. Function names
    /// should match [FunctionDeclaration.name]. When set, model will
    /// predict a function call from only allowed function names.
    /// </summary>
    [JsonPropertyName("allowedFunctionNames")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<string>? AllowedFunctionNames { get; init; }

    /// <summary>
    /// Optional. Specifies the mode in which function calling should execute. If
    /// unspecified, the default value will be set to AUTO.
    /// </summary>
    [JsonPropertyName("mode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FunctionCallingConfigMode? Mode { get; init; }
}

