using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.TunedModels;

/// <summary>
/// A fine-tuned model created using ModelService.CreateTunedModel.
/// </summary>
public sealed record TunedModel
{
    /// <summary>
    /// Immutable. The name of the <see cref="V1Beta.Models.Model"/> to tune.
    /// Example: <c>models/gemini-1.5-flash-001</c>
    /// </summary>
    [JsonPropertyName("baseModel")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? BaseModel { get; init; }

    /// <summary>
    /// Output only. The timestamp when this model was created.
    /// </summary>
    [JsonPropertyName("createTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? CreateTime { get; init; }

    /// <summary>
    /// Optional. A short description of this model.
    /// </summary>
    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Description { get; init; }

    /// <summary>
    /// Optional. The name to display for this model in user interfaces.
    /// The display name must be up to 40 characters including spaces.
    /// </summary>
    [JsonPropertyName("displayName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? DisplayName { get; init; }

    /// <summary>
    /// Output only. The tuned model name. A unique name will be generated on create.
    /// Example: <c>tunedModels/az2mb0bpw6i</c>
    /// If display_name is set on create, the id portion of the name will be set
    /// by concatenating the words of the display_name with hyphens and adding a
    /// random portion for uniqueness.
    /// Example:
    /// * display_name = <c>Sentence Translator</c>
    /// * name = <c>tunedModels/sentence-translator-u3b7m</c>
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Name { get; init; }

    /// <summary>
    /// Optional. List of project numbers that have read access to the tuned model.
    /// </summary>
    [JsonPropertyName("readerProjectNumbers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<long> ReaderProjectNumbers { get; init; }

    /// <summary>
    /// Output only. The state of the tuned model.
    /// </summary>
    [JsonPropertyName("state")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TunedModelState? State { get; init; }

    /// <summary>
    /// Optional. Controls the randomness of the output.
    /// Values can range over <c>[0.0,1.0]</c>, inclusive. A value closer to <c>1.0</c> will
    /// produce responses that are more varied, while a value closer to <c>0.0</c> will
    /// typically result in less surprising responses from the model.
    /// This value specifies default to be the one used by the base model while
    /// creating the model.
    /// </summary>
    [JsonPropertyName("temperature")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float? Temperature { get; init; }

    /// <summary>
    /// Optional. For Top-k sampling.
    /// Top-k sampling considers the set of <c>top_k</c> most probable tokens.
    /// This value specifies default to be used by the backend while making the
    /// call to the model.
    /// This value specifies default to be the one used by the base model while
    /// creating the model.
    /// </summary>
    [JsonPropertyName("topK")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? TopK { get; init; }

    /// <summary>
    /// Optional. For Nucleus sampling.
    /// Nucleus sampling considers the smallest set of tokens whose probability
    /// sum is at least <c>top_p</c>.
    /// This value specifies default to be the one used by the base model while
    /// creating the model.
    /// </summary>
    [JsonPropertyName("topP")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float? TopP { get; init; }

    /// <summary>
    /// Optional. TunedModel to use as the starting point for training the new model.
    /// </summary>
    [JsonPropertyName("tunedModelSource")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TunedModelSource? TunedModelSource { get; init; }

    /// <summary>
    /// Required. The tuning task that creates the tuned model.
    /// </summary>
    [JsonPropertyName("tuningTask")]
    public required TuningTask TuningTask { get; init; }

    /// <summary>
    /// Output only. The timestamp when this model was updated.
    /// </summary>
    [JsonPropertyName("updateTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? UpdateTime { get; init; }
}

