using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.EnvironmentsList;

/// <summary>
/// Response for <c>ListEnvironments</c>.
/// </summary>
public sealed record ListEnvironmentsResponse
{
    /// <summary>
    /// Environments belonging to the provided project.
    /// </summary>
    [JsonPropertyName("environments")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Environment>? Environments { get; init; }

    /// <summary>
    /// Pagination token.
    /// </summary>
    [JsonPropertyName("nextPageToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? NextPageToken { get; init; }
}

