using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Environments;

/// <summary>
/// Network egress mode. Set via the <c>"allowlist": "disabled"</c> short form;
/// must be the only rule in the allowlist and cannot be combined with
/// <c>domain</c>, <c>transform</c> or <c>credential</c>.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<EgressRuleMode>))]
public enum EgressRuleMode
{
    /// <summary>
    /// Default value. Unused.
    /// </summary>
    [JsonStringEnumMemberName("NETWORK_MODE_UNSPECIFIED")]
    NetworkModeUnspecified,

    /// <summary>
    /// All network egress is blocked.
    /// </summary>
    [JsonStringEnumMemberName("DISABLED")]
    Disabled,
}

