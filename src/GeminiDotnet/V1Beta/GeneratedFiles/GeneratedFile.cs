using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.GeneratedFiles;

/// <summary>
/// A file generated on behalf of a user.
/// </summary>
public sealed record GeneratedFile
{
    /// <summary>
    /// Error details if the GeneratedFile ends up in the STATE_FAILED state.
    /// </summary>
    [JsonPropertyName("error")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Status? Error { get; init; }

    /// <summary>
    /// MIME type of the generatedFile.
    /// </summary>
    [JsonPropertyName("mimeType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? MimeType { get; init; }

    /// <summary>
    /// Identifier. The name of the generated file.
    /// Example: <c>generatedFiles/abc-123</c>
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Name { get; init; }

    /// <summary>
    /// Output only. The state of the GeneratedFile.
    /// </summary>
    [JsonPropertyName("state")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public GeneratedFileState? State { get; init; }
}

