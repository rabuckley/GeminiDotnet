using System.Text.Json.Serialization;
using GeminiDotnet.V1Beta;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// This resource represents a long-running operation where metadata and response fields are strongly typed.
/// </summary>
public sealed record PredictLongRunningOperation : BaseOperation
{
    /// <summary>
    /// Metadata for PredictLongRunning long running operations.
    /// </summary>
    [JsonPropertyName("metadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PredictLongRunningMetadata? Metadata { get; init; }

    /// <summary>
    /// Response message for [PredictionService.PredictLongRunning]
    /// </summary>
    [JsonPropertyName("response")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PredictLongRunningResponse? Response { get; init; }
}

