using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.GeneratedFiles;

/// <summary>
/// Response for <c>ListGeneratedFiles</c>.
/// </summary>
public sealed record ListGeneratedFilesResponse
{
    /// <summary>
    /// The list of <see cref="V1Beta.GeneratedFiles.GeneratedFile"/>s.
    /// </summary>
    [JsonPropertyName("generatedFiles")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<GeneratedFile>? GeneratedFiles { get; init; }

    /// <summary>
    /// A token that can be sent as a <c>page_token</c> into a subsequent
    /// <c>ListGeneratedFiles</c> call.
    /// </summary>
    [JsonPropertyName("nextPageToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? NextPageToken { get; init; }
}

