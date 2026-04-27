using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

[JsonConverter(typeof(JsonStringEnumConverter<ServiceTier>))]
public enum ServiceTier
{
    /// <summary>
    /// Default service tier, which is standard.
    /// </summary>
    [JsonStringEnumMemberName("unspecified")]
    Unspecified,

    /// <summary>
    /// Standard service tier.
    /// </summary>
    [JsonStringEnumMemberName("standard")]
    Standard,

    /// <summary>
    /// Flex service tier.
    /// </summary>
    [JsonStringEnumMemberName("flex")]
    Flex,

    /// <summary>
    /// Priority service tier.
    /// </summary>
    [JsonStringEnumMemberName("priority")]
    Priority,
}

