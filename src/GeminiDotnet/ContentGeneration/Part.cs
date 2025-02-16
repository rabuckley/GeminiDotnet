using GeminiDotnet.ContentGeneration.FunctionCalling;
using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// A datatype containing media that is part of a multipart Content message.
/// </summary>
public sealed record Part
{
    /// <summary>
    /// Inline text.
    /// </summary>
    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text { get; init; }

    /// <summary>
    /// Inline media bytes.
    /// </summary>
    [JsonPropertyName("inlineData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Blob? InlineData { get; init; }

    /// <summary>
    /// A predicted FunctionCall returned from the model that contains a string representing the FunctionDeclaration.name with the arguments and their values.
    /// </summary>
    [JsonPropertyName("functionCall")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FunctionCall? FunctionCall { get; init; }

    /// <summary>
    /// The result output of a FunctionCall that contains a string representing the FunctionDeclaration.name and a structured JSON object containing any output from the function is used as context to the model.
    /// </summary>
    [JsonPropertyName("functionResponse")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FunctionResponse? FunctionResponse { get; init; }

    /// <summary>
    /// URI based data.
    /// </summary>
    [JsonPropertyName("fileData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FileData? FileData { get; init; }

    /// <summary>
    /// Code generated by the model that is meant to be executed.
    /// </summary>
    [JsonPropertyName("executableCode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ExecutableCode? ExecutableCode { get; init; }

    /// <summary>
    /// Result of executing the ExecutableCode.
    /// </summary>
    [JsonPropertyName("codeExecutionResult")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CodeExecutionResult? CodeExecutionResult { get; init; }
}
