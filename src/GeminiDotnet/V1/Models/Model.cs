using System.Text.Json.Serialization;

namespace GeminiDotnet.V1.Models;

/// <summary>
/// Information about a Generative Language Model.
/// </summary>
public sealed record Model
{
    /// <summary>
    /// Required. The name of the base model, pass this to the generation request.
    /// Examples:
    /// * <c>gemini-1.5-flash</c>
    /// </summary>
    [JsonPropertyName("baseModelId")]
    public required string BaseModelId { get; init; }

    /// <summary>
    /// A short description of the model.
    /// </summary>
    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Description { get; init; }

    /// <summary>
    /// The human-readable name of the model. E.g. "Gemini 1.5 Flash".
    /// The name can be up to 128 characters long and can consist of any UTF-8
    /// characters.
    /// </summary>
    [JsonPropertyName("displayName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? DisplayName { get; init; }

    /// <summary>
    /// Maximum number of input tokens allowed for this model.
    /// </summary>
    [JsonPropertyName("inputTokenLimit")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? InputTokenLimit { get; init; }

    /// <summary>
    /// The maximum temperature this model can use.
    /// </summary>
    [JsonPropertyName("maxTemperature")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float? MaxTemperature { get; init; }

    /// <summary>
    /// Required. The resource name of the <see cref="V1.Models.Model"/>. Refer to [Model
    /// variants](https://ai.google.dev/gemini-api/docs/models/gemini#model-variations)
    /// for all allowed values.
    /// Format: <c>models/{model}</c> with a <c>{model}</c> naming convention of:
    /// * "{base_model_id}-{version}"
    /// Examples:
    /// * <c>models/gemini-1.5-flash-001</c>
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Maximum number of output tokens available for this model.
    /// </summary>
    [JsonPropertyName("outputTokenLimit")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? OutputTokenLimit { get; init; }

    /// <summary>
    /// The model's supported generation methods.
    /// The corresponding API method names are defined as Pascal case
    /// strings, such as <c>generateMessage</c> and <c>generateContent</c>.
    /// </summary>
    [JsonPropertyName("supportedGenerationMethods")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<string>? SupportedGenerationMethods { get; init; }

    /// <summary>
    /// Controls the randomness of the output.
    /// Values can range over <c>[0.0,max_temperature]</c>, inclusive. A higher value
    /// will produce responses that are more varied, while a value closer to <c>0.0</c>
    /// will typically result in less surprising responses from the model.
    /// This value specifies default to be used by the backend while making the
    /// call to the model.
    /// </summary>
    [JsonPropertyName("temperature")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float? Temperature { get; init; }

    /// <summary>
    /// Whether the model supports thinking.
    /// </summary>
    [JsonPropertyName("thinking")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? Thinking { get; init; }

    /// <summary>
    /// For Top-k sampling.
    /// Top-k sampling considers the set of <c>top_k</c> most probable tokens.
    /// This value specifies default to be used by the backend while making the
    /// call to the model.
    /// If empty, indicates the model doesn't use top-k sampling, and <c>top_k</c> isn't
    /// allowed as a generation parameter.
    /// </summary>
    [JsonPropertyName("topK")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? TopK { get; init; }

    /// <summary>
    /// For [Nucleus
    /// sampling](https://ai.google.dev/gemini-api/docs/prompting-strategies#top-p).
    /// Nucleus sampling considers the smallest set of tokens whose probability
    /// sum is at least <c>top_p</c>.
    /// This value specifies default to be used by the backend while making the
    /// call to the model.
    /// </summary>
    [JsonPropertyName("topP")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float? TopP { get; init; }

    /// <summary>
    /// Required. The version number of the model.
    /// This represents the major version (<c>1.0</c> or <c>1.5</c>)
    /// </summary>
    [JsonPropertyName("version")]
    public required string Version { get; init; }
}

