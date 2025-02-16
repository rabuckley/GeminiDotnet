using GeminiDotnet.ContentGeneration.FunctionCalling;
using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Defines the execution behavior for function calling by defining the execution mode.
/// </summary>
public enum FunctionCallingMode
{
    /// <summary>
    /// Unspecified function calling mode. This value should not be used.
    /// </summary>
    [JsonStringEnumMemberName("UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Default model behavior, model decides to predict either a function call or a natural language response.
    /// </summary>
    [JsonStringEnumMemberName("AUTO")]
    Auto,

    /// <summary>
    /// Model is constrained to always predicting a function call only. If <see cref="FunctionCallingConfiguration.AllowedFunctionNames"/>
    /// are set, the predicted function call will be limited to any one of them, else the predicted function call will
    /// be any one of the provided <see cref="Tool.FunctionDeclarations"/>.
    /// </summary>
    [JsonStringEnumMemberName("ANY")]
    Any,

    /// <summary>
    /// Model will not predict any function call. Model behavior is same as when not passing any function declarations.
    /// </summary>
    [JsonStringEnumMemberName("NONE")]
    None
}
