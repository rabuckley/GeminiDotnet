using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Response from <c>ListPermissions</c> containing a paginated list of
/// permissions.
/// </summary>
public sealed record ListPermissionsResponse
{
    /// <summary>
    /// A token, which can be sent as <c>page_token</c> to retrieve the next page.
    /// If this field is omitted, there are no more pages.
    /// </summary>
    [JsonPropertyName("nextPageToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? NextPageToken { get; init; }

    /// <summary>
    /// Returned permissions.
    /// </summary>
    [JsonPropertyName("permissions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Permission>? Permissions { get; init; }
}

