using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.CachedContents;

/// <summary>
/// Tool details that the model may use to generate response.
/// A <see cref="V1Beta.CachedContents.Tool"/> is a piece of code that enables the system to interact with
/// external systems to perform an action, or set of actions, outside of
/// knowledge and scope of the model.
/// Next ID: 14
/// </summary>
public sealed record Tool
{
    /// <summary>
    /// Optional. Enables the model to execute code as part of generation.
    /// </summary>
    [JsonPropertyName("codeExecution")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CodeExecution? CodeExecution { get; init; }

    /// <summary>
    /// Optional. Tool to support the model interacting directly with the computer.
    /// If enabled, it automatically populates computer-use specific Function
    /// Declarations.
    /// </summary>
    [JsonPropertyName("computerUse")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ComputerUse? ComputerUse { get; init; }

    /// <summary>
    /// Optional. FileSearch tool type.
    /// Tool to retrieve knowledge from Semantic Retrieval corpora.
    /// </summary>
    [JsonPropertyName("fileSearch")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FileSearch? FileSearch { get; init; }

    /// <summary>
    /// Optional. A list of <c>FunctionDeclarations</c> available to the model that can be used
    /// for function calling.
    /// The model or system does not execute the function. Instead the defined
    /// function may be returned as a FunctionCall
    /// with arguments to the client side for execution. The model may decide to
    /// call a subset of these functions by populating
    /// FunctionCall in the response. The next
    /// conversation turn may contain a
    /// FunctionResponse
    /// with the Content.role "function" generation context for the next model
    /// turn.
    /// </summary>
    [JsonPropertyName("functionDeclarations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<FunctionDeclaration>? FunctionDeclarations { get; init; }

    /// <summary>
    /// Optional. Tool that allows grounding the model's response with geospatial context
    /// related to the user's query.
    /// </summary>
    [JsonPropertyName("googleMaps")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public GoogleMaps? GoogleMaps { get; init; }

    /// <summary>
    /// Optional. GoogleSearch tool type.
    /// Tool to support Google Search in Model. Powered by Google.
    /// </summary>
    [JsonPropertyName("googleSearch")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public GoogleSearch? GoogleSearch { get; init; }

    /// <summary>
    /// Optional. Retrieval tool that is powered by Google search.
    /// </summary>
    [JsonPropertyName("googleSearchRetrieval")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public GoogleSearchRetrieval? GoogleSearchRetrieval { get; init; }

    /// <summary>
    /// Optional. MCP Servers to connect to.
    /// </summary>
    [JsonPropertyName("mcpServers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<McpServer>? McpServers { get; init; }

    /// <summary>
    /// Optional. Tool to support URL context retrieval.
    /// </summary>
    [JsonPropertyName("urlContext")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public UrlContext? UrlContext { get; init; }
}

