using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.CachedContents;

/// <summary>
/// Computer Use tool type.
/// </summary>
public sealed record ComputerUse
{
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

