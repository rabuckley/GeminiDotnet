using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Environments;

/// <summary>
/// Response for <c>GetEnvironmentFiles</c>.
/// </summary>
public sealed record GetEnvironmentFilesResponse
{
    /// <summary>
    /// If the requested path is a directory, this contains its contents.
    /// If the requested path is a file, this contains a single entry with the
    /// file's metadata.
    /// If alt=media was specified, this is empty (content is served via <c>blob</c>).
    /// </summary>
    [JsonPropertyName("files")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<EnvironmentFile>? Files { get; init; }

    /// <summary>
    /// Pagination token for directory listing.
    /// NOLINT
    /// </summary>
    [JsonPropertyName("next_page_token")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? NextPageToken { get; init; }
}

