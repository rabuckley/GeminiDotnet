using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.AuthTokens;

/// <summary>
/// Message to be sent in the first (and only in the first)
/// <c>BidiGenerateContentClientMessage</c>. Contains configuration that will apply
/// for the duration of the streaming RPC.
/// Clients should wait for a <c>BidiGenerateContentSetupComplete</c> message before
/// sending any additional messages.
/// </summary>
public sealed record BidiGenerateContentSetup
{
    /// <summary>
    /// Optional. Configures a context window compression mechanism.
    /// If included, the server will automatically reduce the size of the context
    /// when it exceeds the configured length.
    /// </summary>
    [JsonPropertyName("contextWindowCompression")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ContextWindowCompressionConfiguration? ContextWindowCompression { get; init; }

    /// <summary>
    /// Optional. Generation config.
    /// The following fields are not supported:
    /// - <c>response_logprobs</c>
    /// - <c>response_mime_type</c>
    /// - <c>logprobs</c>
    /// - <c>response_schema</c>
    /// - <c>response_json_schema</c>
    /// - <c>stop_sequence</c>
    /// - <c>skip_response_cache</c>
    /// - <c>routing_config</c>
    /// - <c>audio_timestamp</c>
    /// </summary>
    [JsonPropertyName("generationConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public GenerationConfiguration? GenerationConfiguration { get; init; }

    /// <summary>
    /// Optional. Configures the exchange of history between the client and the server.
    /// </summary>
    [JsonPropertyName("historyConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public HistoryConfiguration? HistoryConfiguration { get; init; }

    /// <summary>
    /// Optional. If set, enables transcription of voice input. The transcription aligns
    /// with the input audio language, if configured.
    /// </summary>
    [JsonPropertyName("inputAudioTranscription")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public AudioTranscriptionConfiguration? InputAudioTranscription { get; init; }

    /// <summary>
    /// Required. The model's resource name. This serves as an ID for the Model to use.
    /// Format: <c>models/{model}</c>
    /// </summary>
    [JsonPropertyName("model")]
    public required string Model { get; init; }

    /// <summary>
    /// Optional. If set, enables transcription of the model's audio output. The
    /// transcription aligns with the language code specified for the output
    /// audio, if configured.
    /// </summary>
    [JsonPropertyName("outputAudioTranscription")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public AudioTranscriptionConfiguration? OutputAudioTranscription { get; init; }

    /// <summary>
    /// Optional. Configures the handling of realtime input.
    /// </summary>
    [JsonPropertyName("realtimeInputConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public RealtimeInputConfiguration? RealtimeInputConfiguration { get; init; }

    /// <summary>
    /// Optional. Configures session resumption mechanism.
    /// If included, the server will send <c>SessionResumptionUpdate</c> messages.
    /// </summary>
    [JsonPropertyName("sessionResumption")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SessionResumptionConfiguration? SessionResumption { get; init; }

    /// <summary>
    /// Optional. The user provided system instructions for the model.
    /// Note: Only text should be used in parts and content in each part will be
    /// in a separate paragraph.
    /// </summary>
    [JsonPropertyName("systemInstruction")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Content? SystemInstruction { get; init; }

    /// <summary>
    /// Optional. A list of <c>Tools</c> the model may use to generate the next response.
    /// A <see cref="V1Beta.Tool"/> is a piece of code that enables the system to interact with
    /// external systems to perform an action, or set of actions, outside of
    /// knowledge and scope of the model.
    /// </summary>
    [JsonPropertyName("tools")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Tool>? Tools { get; init; }
}

