using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Computer Use tool type.
/// </summary>
public sealed record ComputerUse
{
    /// <summary>
    /// Optional. Disabled safety policies for computer use.
    /// </summary>
    [JsonPropertyName("disabledSafetyPolicies")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<ComputerUseDisabledSafetyPolicies>? DisabledSafetyPolicies { get; init; }

    /// <summary>
    /// Optional. Whether enable the prompt injection detection check on computer-use
    /// request.
    /// </summary>
    [JsonPropertyName("enablePromptInjectionDetection")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? EnablePromptInjectionDetection { get; init; }

    /// <summary>
    /// Required. The environment being operated.
    /// </summary>
    [JsonPropertyName("environment")]
    public required ComputerUseEnvironment Environment { get; init; }

    /// <summary>
    /// Optional. By default, predefined functions are included in the final model
    /// call.
    /// Some of them can be explicitly excluded from being automatically
    /// included. This can serve two purposes:
    /// 1. Using a more restricted / different action space.
    /// 2. Improving the definitions / instructions of predefined functions.
    /// </summary>
    [JsonPropertyName("excludedPredefinedFunctions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<string>? ExcludedPredefinedFunctions { get; init; }
}

