using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Batches;

/// <summary>
/// Configures the input to the batch request.
/// </summary>
public sealed record InputConfiguration
{
    /// <summary>
    /// The name of the <see cref="V1Beta.Files.File"/> containing the input requests.
    /// </summary>
    [JsonPropertyName("fileName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? FileName { get; init; }

    /// <summary>
    /// The requests to be processed in the batch.
    /// </summary>
    [JsonPropertyName("requests")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public InlinedRequests? Requests { get; init; }
}

