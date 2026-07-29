using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Configures the input to the batch request.
/// </summary>
public sealed record InputEmbedContentConfiguration
{
    /// <summary>
    /// The name of the <see cref="V1.Files.File"/> containing the input requests.
    /// </summary>
    [JsonPropertyName("fileName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? FileName { get; init; }

    /// <summary>
    /// The requests to be processed in the batch.
    /// </summary>
    [JsonPropertyName("requests")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public InlinedEmbedContentRequests? Requests { get; init; }
}

