using System.Collections.Generic;
using System.Text.Json.Serialization;

using GeminiDotnet.ContentGeneration.FunctionCalling;
using GeminiDotnet.ContentGeneration.Safety;

namespace GeminiDotnet.ContentGeneration;

public sealed record GenerateContentRequest
{
    [JsonPropertyName("generation_config")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public GenerationConfiguration? GenerationConfig { get; init; }

    [JsonPropertyName("systemInstruction")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ChatMessage? SystemInstruction { get; init; }

    [JsonPropertyName("contents")]
    public required IEnumerable<ChatMessage> Contents { get; init; }

    [JsonPropertyName("safetySettings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<SafetySetting>? SafetySettings { get; init; }

    [JsonPropertyName("tools")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<Tool>? Tools { get; init; }
}