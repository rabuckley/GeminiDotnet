using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.FunctionCalling;

public sealed record Tool
{
    [JsonPropertyName("functionDeclaration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<FunctionDeclaration>? FunctionDeclarations { get; init; }

    [JsonPropertyName("codeExecution")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CodeExecution? CodeExecution { get; init; }
}