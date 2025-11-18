using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Required. Programming language of the `code`.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ExecutableCodeLanguage>))]
public enum ExecutableCodeLanguage
{
    /// <summary>
    /// Unspecified language. This value should not be used.
    /// </summary>
    [JsonStringEnumMemberName("LANGUAGE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Python >= 3.10, with numpy and simpy available.
    /// Python is the default language.
    /// </summary>
    [JsonStringEnumMemberName("PYTHON")]
    Python,
}

