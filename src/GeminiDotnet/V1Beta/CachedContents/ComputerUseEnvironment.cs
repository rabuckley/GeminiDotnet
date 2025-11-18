using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.CachedContents;

/// <summary>
/// Required. The environment being operated.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ComputerUseEnvironment>))]
public enum ComputerUseEnvironment
{
    /// <summary>
    /// Defaults to browser.
    /// </summary>
    [JsonStringEnumMemberName("ENVIRONMENT_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Operates in a web browser.
    /// </summary>
    [JsonStringEnumMemberName("ENVIRONMENT_BROWSER")]
    Browser,
}

