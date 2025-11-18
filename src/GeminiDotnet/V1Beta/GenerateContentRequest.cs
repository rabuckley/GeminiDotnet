using System.Text.Json.Serialization;
using GeminiDotnet.V1Beta.CachedContents;
using GeminiDotnet.V1Beta.Models;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Request to generate a completion from the model.
/// </summary>
public sealed record GenerateContentRequest
{
    /// <summary>
    /// Optional. The name of the content
    /// [cached](https://ai.google.dev/gemini-api/docs/caching) to use as context
    /// to serve the prediction. Format: <c>cachedContents/{cachedContent}</c>
    /// </summary>
    [JsonPropertyName("cachedContent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? CachedContent { get; init; }

    /// <summary>
    /// Required. The content of the current conversation with the model.
    /// For single-turn queries, this is a single instance. For multi-turn queries
    /// like [chat](https://ai.google.dev/gemini-api/docs/text-generation#chat),
    /// this is a repeated field that contains the conversation history and the
    /// latest request.
    /// </summary>
    [JsonPropertyName("contents")]
    public required IReadOnlyList<Content> Contents { get; init; }

    /// <summary>
    /// Optional. Configuration options for model generation and outputs.
    /// </summary>
    [JsonPropertyName("generationConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public GenerationConfiguration? GenerationConfiguration { get; init; }

    /// <summary>
    /// Required. The name of the <see cref="V1Beta.Models.Model"/> to use for generating the completion.
    /// Format: <c>models/{model}</c>.
    /// </summary>
    [JsonPropertyName("model")]
    public required string Model { get; init; }

    /// <summary>
    /// Optional. A list of unique <see cref="V1Beta.Models.SafetySetting"/> instances for blocking unsafe content.
    /// This will be enforced on the <see cref="Contents"/> and
    /// <c>GenerateContentResponse.candidates</c>. There should not be more than one
    /// setting for each <c>SafetyCategory</c> type. The API will block any contents and
    /// responses that fail to meet the thresholds set by these settings. This list
    /// overrides the default settings for each <c>SafetyCategory</c> specified in the
    /// safety_settings. If there is no <see cref="V1Beta.Models.SafetySetting"/> for a given
    /// <c>SafetyCategory</c> provided in the list, the API will use the default safety
    /// setting for that category. Harm categories HARM_CATEGORY_HATE_SPEECH,
    /// HARM_CATEGORY_SEXUALLY_EXPLICIT, HARM_CATEGORY_DANGEROUS_CONTENT,
    /// HARM_CATEGORY_HARASSMENT, HARM_CATEGORY_CIVIC_INTEGRITY are supported.
    /// Refer to the [guide](https://ai.google.dev/gemini-api/docs/safety-settings)
    /// for detailed information on available safety settings. Also refer to the
    /// [Safety guidance](https://ai.google.dev/gemini-api/docs/safety-guidance) to
    /// learn how to incorporate safety considerations in your AI applications.
    /// </summary>
    [JsonPropertyName("safetySettings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<SafetySetting>? SafetySettings { get; init; }

    /// <summary>
    /// Optional. Developer set [system
    /// instruction(s)](https://ai.google.dev/gemini-api/docs/system-instructions).
    /// Currently, text only.
    /// </summary>
    [JsonPropertyName("systemInstruction")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Content? SystemInstruction { get; init; }

    /// <summary>
    /// Optional. Tool configuration for any <see cref="V1Beta.CachedContents.Tool"/> specified in the request. Refer to the
    /// [Function calling
    /// guide](https://ai.google.dev/gemini-api/docs/function-calling#function_calling_mode)
    /// for a usage example.
    /// </summary>
    [JsonPropertyName("toolConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ToolConfiguration? ToolConfiguration { get; init; }

    /// <summary>
    /// Optional. A list of <c>Tools</c> the <see cref="V1Beta.Models.Model"/> may use to generate the next response.
    /// A <see cref="V1Beta.CachedContents.Tool"/> is a piece of code that enables the system to interact with
    /// external systems to perform an action, or set of actions, outside of
    /// knowledge and scope of the <see cref="V1Beta.Models.Model"/>. Supported <see cref="V1Beta.CachedContents.Tool"/>s are <c>Function</c> and
    /// <c>code_execution</c>. Refer to the [Function
    /// calling](https://ai.google.dev/gemini-api/docs/function-calling) and the
    /// [Code execution](https://ai.google.dev/gemini-api/docs/code-execution)
    /// guides to learn more.
    /// </summary>
    [JsonPropertyName("tools")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Tool>? Tools { get; init; }
}

