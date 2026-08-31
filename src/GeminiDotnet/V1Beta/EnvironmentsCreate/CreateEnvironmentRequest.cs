using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.EnvironmentsCreate;

/// <summary>
/// Request for <c>CreateEnvironment</c>.
/// </summary>
public sealed record CreateEnvironmentRequest
{
    /// <summary>
    /// Allow only specific domains.
    /// </summary>
    [JsonPropertyName("networkAllowlist")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public EnvironmentNetworkEgressAllowlist? NetworkAllowlist { get; init; }

    /// <summary>
    /// Network egress mode.
    /// </summary>
    [JsonPropertyName("networkMode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CreateEnvironmentRequestNetworkMode? NetworkMode { get; init; }

    /// <summary>
    /// Sources to be mounted into the environment.
    /// </summary>
    [JsonPropertyName("sources")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Source>? Sources { get; init; }
}

