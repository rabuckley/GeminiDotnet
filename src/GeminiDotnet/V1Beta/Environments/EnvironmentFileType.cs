using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Environments;

/// <summary>
/// Output only. The type of the entry.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<EnvironmentFileType>))]
public enum EnvironmentFileType
{
    /// <summary>
    /// Unspecified type.
    /// </summary>
    [JsonStringEnumMemberName("TYPE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// A regular file.
    /// </summary>
    [JsonStringEnumMemberName("FILE")]
    File,

    /// <summary>
    /// A directory.
    /// </summary>
    [JsonStringEnumMemberName("DIRECTORY")]
    Directory,
}

