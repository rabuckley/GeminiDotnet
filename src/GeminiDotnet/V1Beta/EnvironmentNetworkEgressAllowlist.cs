using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Network egress configuration for the environment.
/// </summary>
public sealed record EnvironmentNetworkEgressAllowlist
{
    /// <summary>
    /// List of allowed domains and their configurations.
    /// </summary>
    [JsonPropertyName("allowlist")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<EgressRule>? Allowlist { get; init; }
}

