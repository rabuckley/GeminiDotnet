using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Files;

/// <summary>
/// Response for <c>ListFiles</c>.
/// </summary>
public sealed record ListFilesResponse
{
    /// <summary>
    /// The list of <see cref="V1Beta.Files.File"/>s.
    /// </summary>
    [JsonPropertyName("files")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<File>? Files { get; init; }

    /// <summary>
    /// A token that can be sent as a <c>page_token</c> into a subsequent <c>ListFiles</c>
    /// call.
    /// </summary>
    [JsonPropertyName("nextPageToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? NextPageToken { get; init; }
}

