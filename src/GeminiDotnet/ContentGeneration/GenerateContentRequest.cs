using GeminiDotnet.ContentGeneration.FunctionCalling;
using GeminiDotnet.ContentGeneration.Safety;
using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

public sealed record GenerateContentRequest
{
    /// <summary>
    /// The content of the current conversation with the model.
    /// For single-turn queries, this is a single instance. For multi-turn queries like chat, this is a repeated field
    /// that contains the conversation history and the latest request.
    /// </summary>
    [JsonPropertyName("contents")]
    public required IReadOnlyList<Content> Contents { get; init; }

    /// <summary>
    /// A list of Tools the Model may use to generate the next response.
    /// A Tool is a piece of code that enables the system to interact with external systems to perform an action, or
    /// set of actions, outside of knowledge and scope of the Model.
    /// </summary>
    [JsonPropertyName("tools")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<Tool>? Tools { get; init; }

    /// <summary>
    /// <see cref="ToolConfiguration"/> for any <see cref="Tool"/> specified in the request.
    /// </summary>
    [JsonPropertyName("toolConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ToolConfiguration? ToolConfiguration { get; init; }

    /// <summary>
    /// A list of unique <see cref="SafetySettings"/> instances for blocking unsafe content.
    /// </summary>
    [JsonPropertyName("safetySettings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<SafetySetting>? SafetySettings { get; init; }

    /// <summary>
    /// Developer set system instruction(s). Currently, text only.
    /// </summary>
    [JsonPropertyName("systemInstruction")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Content? SystemInstruction { get; init; }

    /// <summary>
    /// Configuration options for model generation and outputs.
    /// </summary>
    [JsonPropertyName("generationConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public GenerationConfiguration? GenerationConfiguration { get; init; }

    /// <summary>
    /// The name of the content cached to use as context to serve the prediction.
    /// Format: <c>cachedContents/{cachedContent}</c>
    /// </summary>
    /// <returns></returns>
    [JsonPropertyName("cachedContent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CachedContent { get; init; }
}
