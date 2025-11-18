using System.Text.Json.Serialization;
using GeminiDotnet.V1.Batches;

namespace GeminiDotnet.V1;

/// <summary>
/// This resource represents a long-running operation that is the result of a
/// network API call.
/// </summary>
public record BaseOperation
{
    /// <summary>
    /// If the value is <c>false</c>, it means the operation is still in progress.
    /// If <c>true</c>, the operation is completed, and either <see cref="Error"/> or <c>response</c> is
    /// available.
    /// </summary>
    [JsonPropertyName("done")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? Done { get; init; }

    /// <summary>
    /// The error result of the operation in case of failure or cancellation.
    /// </summary>
    [JsonPropertyName("error")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Status? Error { get; init; }

    /// <summary>
    /// The server-assigned name, which is only unique within the same service that
    /// originally returns it. If you use the default HTTP mapping, the
    /// <see cref="Name"/> should be a resource name ending with <c>operations/{unique_id}</c>.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Name { get; init; }
}

