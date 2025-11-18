using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.GeneratedFiles;

/// <summary>
/// Output only. The state of the GeneratedFile.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<GeneratedFileState>))]
public enum GeneratedFileState
{
    /// <summary>
    /// The default value. This value is used if the state is omitted.
    /// </summary>
    [JsonStringEnumMemberName("STATE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Being generated.
    /// </summary>
    [JsonStringEnumMemberName("GENERATING")]
    Generating,

    /// <summary>
    /// Generated and is ready for download.
    /// </summary>
    [JsonStringEnumMemberName("GENERATED")]
    Generated,

    /// <summary>
    /// Failed to generate the GeneratedFile.
    /// </summary>
    [JsonStringEnumMemberName("FAILED")]
    Failed,
}

