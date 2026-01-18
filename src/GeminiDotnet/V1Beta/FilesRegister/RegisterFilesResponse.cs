using System.Text.Json.Serialization;
using File = GeminiDotnet.V1Beta.Files.File;

namespace GeminiDotnet.V1Beta.FilesRegister;

/// <summary>
/// Response for <c>RegisterFiles</c>.
/// </summary>
public sealed record RegisterFilesResponse
{
    /// <summary>
    /// The registered files to be used when calling GenerateContent.
    /// </summary>
    [JsonPropertyName("files")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<File>? Files { get; init; }
}

