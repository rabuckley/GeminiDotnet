using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Environments;

/// <summary>
/// Response for <c>UploadEnvironmentFile</c>.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record UploadEnvironmentFileResponse
{
    /// <summary>
    /// List of files created or extracted in the environment.
    /// </summary>
    [JsonPropertyName("files")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<EnvironmentFile>? Files { get; init; }
}

