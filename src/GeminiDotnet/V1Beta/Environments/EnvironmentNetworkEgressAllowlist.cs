using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Environments;

/// <summary>
/// Network egress configuration for the environment.
/// </summary>
public sealed record EnvironmentNetworkEgressAllowlist
{
    /// <summary>
    /// List of allowed domains and their configurations. Set to <c>"disabled"</c>
    /// to block all network egress.
    /// </summary>
    [JsonPropertyName("allowlist")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<EgressRule>? Allowlist { get; init; }
}

