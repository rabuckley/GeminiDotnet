using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.EnvironmentsCreate;

/// <summary>
/// Network egress mode.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<CreateEnvironmentRequestNetworkMode>))]
public enum CreateEnvironmentRequestNetworkMode
{
    /// <summary>
    /// Default value. Unused.
    /// </summary>
    [JsonStringEnumMemberName("NETWORK_MODE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// All network egress is blocked.
    /// </summary>
    [JsonStringEnumMemberName("DISABLED")]
    Disabled,
}

