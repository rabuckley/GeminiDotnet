using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.FunctionCalling;

/// <summary>
/// Structured representation of a function declaration as defined by the <see href="https://spec.openapis.org/oas/v3.0.3">OpenAPI 3.03 specification</see>. Included in this declaration are the function name and parameters. This <see cref="FunctionDeclaration"/> is a representation of a block of code that can be used as a <see cref="Tool"/> by the model and executed by the client.
/// </summary>
public sealed record FunctionDeclaration
{
    /// <summary>
    /// The name of the function. Must be a-z, A-Z, 0-9, or contain underscores and dashes, with a maximum length of 63.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// A brief description of the function.
    /// </summary>
    [JsonPropertyName("description")]
    public required string Description { get; init; }

    /// <summary>
    /// Optional. Specifies the function Behavior. Currently only supported by the BidiGenerateContent method.
    /// </summary>
    [JsonPropertyName("behavior")]
    public FunctionBehavior? Behavior { get; init; }

    /// <summary>
    /// Describes the parameters to this function. Reflects the Open API 3.03 Parameter Object string Key: the name of the parameter. Parameter names are case-sensitive. Schema Value: the Schema defining the type used for the parameter.
    /// </summary>
    [JsonPropertyName("parameters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Schema? Parameters { get; init; }

    /// <summary>
    /// Optional. Describes the parameters to the function in JSON Schema format. The schema must describe an object where the properties are the parameters to the function.
    /// <remarks>
    /// This field is mutually exclusive with <see cref="Parameters"/>.
    /// </remarks>
    /// </summary>
    [JsonPropertyName("parametersJsonSchema")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JsonElement ParametersJsonSchema { get; init; }

    /// <summary>
    /// Describes the output from this function in JSON Schema format. Reflects the Open API 3.03 Response Object. The Schema defines the type used for the response value of the function.
    /// </summary>
    [JsonPropertyName("response")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Schema? Response { get; init; }

    /// <summary>
    /// Optional. Describes the output from this function in JSON Schema format. The value specified by the schema is the response value of the function.
    /// <remarks>
    /// This field is mutually exclusive with response.
    /// </remarks>
    /// </summary>
    [JsonPropertyName("responseJsonSchema")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JsonElement ResponseJsonSchema { get; init; }
}
