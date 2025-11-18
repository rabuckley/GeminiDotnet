using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.CachedContents;

/// <summary>
/// Optional. Specifies the mode in which function calling should execute. If
/// unspecified, the default value will be set to AUTO.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<FunctionCallingConfigMode>))]
public enum FunctionCallingConfigMode
{
    /// <summary>
    /// Unspecified function calling mode. This value should not be used.
    /// </summary>
    [JsonStringEnumMemberName("MODE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Default model behavior, model decides to predict either a function call
    /// or a natural language response.
    /// </summary>
    [JsonStringEnumMemberName("AUTO")]
    Auto,

    /// <summary>
    /// Model is constrained to always predicting a function call only.
    /// If "allowed_function_names" are set, the predicted function call will be
    /// limited to any one of "allowed_function_names", else the predicted
    /// function call will be any one of the provided "function_declarations".
    /// </summary>
    [JsonStringEnumMemberName("ANY")]
    Any,

    /// <summary>
    /// Model will not predict any function call. Model behavior is same as when
    /// not passing any function declarations.
    /// </summary>
    [JsonStringEnumMemberName("NONE")]
    None,

    /// <summary>
    /// Model decides to predict either a function call
    /// or a natural language response, but will validate function calls with
    /// constrained decoding.
    /// If "allowed_function_names" are set, the predicted function call will be
    /// limited to any one of "allowed_function_names", else the predicted
    /// function call will be any one of the provided "function_declarations".
    /// </summary>
    [JsonStringEnumMemberName("VALIDATED")]
    Validated,
}

