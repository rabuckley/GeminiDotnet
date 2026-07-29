using System.Text.Json.Serialization;
using GeminiDotnet.V1Beta;

namespace GeminiDotnet.V1Beta.TunedModels;

/// <summary>
/// This resource represents a long-running operation where metadata and response fields are strongly typed.
/// </summary>
public sealed record CreateTunedModelOperation : BaseOperation
{
    /// <summary>
    /// Metadata about the state and progress of creating a tuned model returned from
    /// the long-running operation
    /// </summary>
    [JsonPropertyName("metadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CreateTunedModelMetadata? Metadata { get; init; }

    /// <summary>
    /// A fine-tuned model created using ModelService.CreateTunedModel.
    /// </summary>
    [JsonPropertyName("response")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TunedModel? Response { get; init; }
}

