using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Environments;

/// <summary>
/// A single domain allowlist rule with optional header injection.
/// </summary>
public sealed record EgressRule
{
    /// <summary>
    /// Optional. Reference to a server-managed Credential resource by ID.
    /// </summary>
    [JsonPropertyName("credential")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Credential { get; init; }

    /// <summary>
    /// Domain to allow outbound requests to. Supports wildcards (e.g.
    /// '*.googleapis.com'). Use '*' to allow all domains.
    /// </summary>
    [JsonPropertyName("domain")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Domain { get; init; }

    /// <summary>
    /// Network egress mode. Set via the <c>"allowlist": "disabled"</c> short form;
    /// must be the only rule in the allowlist and cannot be combined with
    /// <see cref="Domain"/>, <see cref="Transform"/> or <see cref="Credential"/>.
    /// </summary>
    [JsonPropertyName("mode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public EgressRuleMode? Mode { get; init; }

    /// <summary>
    /// Headers to inject into requests matching this rule.
    /// Key: header name (e.g., "Authorization").
    /// Value: header value (e.g., "Bearer your-token").
    /// </summary>
    [JsonPropertyName("transform")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyDictionary<string, string>? Transform { get; init; }
}

