using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Files;

/// <summary>
/// Output only. Processing state of the File.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<FileState>))]
public enum FileState
{
    /// <summary>
    /// The default value. This value is used if the state is omitted.
    /// </summary>
    [JsonStringEnumMemberName("STATE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// File is being processed and cannot be used for inference yet.
    /// </summary>
    [JsonStringEnumMemberName("PROCESSING")]
    Processing,

    /// <summary>
    /// File is processed and available for inference.
    /// </summary>
    [JsonStringEnumMemberName("ACTIVE")]
    Active,

    /// <summary>
    /// File failed processing.
    /// </summary>
    [JsonStringEnumMemberName("FAILED")]
    Failed,
}

