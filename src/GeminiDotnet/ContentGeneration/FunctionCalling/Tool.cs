using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.FunctionCalling;

/// <summary>
/// Tool details that the model may use to generate response.
/// A Tool is a piece of code that enables the system to interact with external systems to perform an action, or set of
/// actions, outside of knowledge and scope of the model.
/// </summary>
public sealed record Tool
{
    /// <summary>
    ///  list of FunctionDeclarations available to the model that can be used for function calling. The model or system
    /// does not execute the function. Instead, the defined function may be returned as a <see cref="FunctionCall"/>
    /// with arguments to the client side for execution. The model may decide to call a subset of these functions by
    /// populating <see cref="FunctionCall"/> in the response. The next conversation turn may contain a
    /// <see cref="FunctionResponse"/> with the <see cref="Content.Role"/> "function" generation context for the next
    /// model turn.
    /// </summary>
    [JsonPropertyName("functionDeclaration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<FunctionDeclaration>? FunctionDeclarations { get; init; }

    /// <summary>
    /// Retrieval tool that is powered by Google search.
    /// </summary>
    [JsonPropertyName("googleSearchRetrieval")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public GoogleSearchRetrieval? GoogleSearchRetrieval { get; init; }

    /// <summary>
    /// Enables the model to execute code as part of generation.
    /// </summary>
    [JsonPropertyName("codeExecution")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CodeExecution? CodeExecution { get; init; }

    /// <summary>
    /// GoogleSearch tool type. Tool to support Google Search in Model. Powered by Google.
    /// </summary>
    [JsonPropertyName("googleSearch")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public GoogleSearch? GoogleSearch { get; init; }
}
