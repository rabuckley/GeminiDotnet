using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// A network egress rule that controls which external domains the
/// environment is allowed to reach.  Each rule identifies a target domain
/// and, optionally, a set of HTTP headers to inject into every matching
/// outbound request.
/// </summary>
public sealed record EgressRule
{
    /// <summary>
    /// The domain pattern to match for this rule.
    /// Use an exact hostname (e.g., <c>github.com</c>), a wildcard prefix
    /// (e.g., <c>*.googleapis.com</c>), or <c>*</c> to match all domains.
    /// </summary>
    [JsonPropertyName("domain")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Domain { get; init; }

    /// <summary>
    /// Headers to inject into requests matching this rule.
    /// Key: header name (e.g., "Authorization").
    /// Value: header value (e.g., "Bearer your-token").
    /// </summary>
    [JsonPropertyName("transform")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyDictionary<string, string>? Transform { get; init; }
}

