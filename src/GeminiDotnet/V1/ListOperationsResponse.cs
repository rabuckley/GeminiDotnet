using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// The response message for Operations.ListOperations.
/// </summary>
public sealed record ListOperationsResponse
{
    /// <summary>
    /// The standard List next-page token.
    /// </summary>
    [JsonPropertyName("nextPageToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? NextPageToken { get; init; }

    /// <summary>
    /// A list of operations that matches the specified filter in the request.
    /// </summary>
    [JsonPropertyName("operations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Operation>? Operations { get; init; }

    /// <summary>
    /// Unordered list. Unreachable resources. Populated when the request sets
    /// <c>ListOperationsRequest.return_partial_success</c> and reads across
    /// collections. For example, when attempting to list all resources across all
    /// supported locations.
    /// </summary>
    [JsonPropertyName("unreachable")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<string>? Unreachable { get; init; }
}

