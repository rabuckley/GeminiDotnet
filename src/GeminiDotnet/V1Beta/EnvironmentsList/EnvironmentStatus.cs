using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.EnvironmentsList;

/// <summary>
/// Output only. The status of the environment container.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<EnvironmentStatus>))]
public enum EnvironmentStatus
{
    [JsonStringEnumMemberName("STATUS_UNSPECIFIED")]
    Unspecified,

    [JsonStringEnumMemberName("ACTIVE")]
    Active,

    [JsonStringEnumMemberName("EXPIRED")]
    Expired,
}

