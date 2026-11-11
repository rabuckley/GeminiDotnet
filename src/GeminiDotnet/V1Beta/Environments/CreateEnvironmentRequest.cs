using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Environments;

/// <summary>
/// Request for <c>CreateEnvironment</c>.
/// </summary>
public sealed record CreateEnvironmentRequest
{
    /// <summary>
    /// Optional. Arbitrary customer-defined JSON configuration passed through
    /// to the self-hosted environment poller. Only supported when <see cref="Mode"/> is
    /// <c>SELF_HOSTED</c>.
    /// </summary>
    [JsonPropertyName("customConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JsonElement CustomConfiguration { get; init; }

    /// <summary>
    /// Optional. The source environment to copy/fork from.
    /// Format: <c>environments/{environment_id}</c> or <c>{environment_id}</c>.
    /// When specified, <see cref="Sources"/> and <c>env</c> must be empty.
    /// </summary>
    [JsonPropertyName("fromEnvironment")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? FromEnvironment { get; init; }

    /// <summary>
    /// The compute lifecycle mode for this environment.
    /// </summary>
    [JsonPropertyName("mode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CreateEnvironmentRequestMode? Mode { get; init; }

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

    /// <summary>
    /// Optional. System instruction applied to interactions running in this
    /// environment. Only supported for <c>SELF_HOSTED</c> environments, where it is
    /// recommended.
    /// </summary>
    [JsonPropertyName("systemInstruction")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? SystemInstruction { get; init; }
}

