using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Content filtering metadata associated with processing a single request.
/// ContentFilter contains a reason and an optional supporting string. The reason
/// may be unspecified.
/// </summary>
public sealed record ContentFilter
{
    /// <summary>
    /// A string that describes the filtering behavior in more detail.
    /// </summary>
    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Message { get; init; }

    /// <summary>
    /// The reason content was blocked during request processing.
    /// </summary>
    [JsonPropertyName("reason")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ContentFilterReason? Reason { get; init; }
}

