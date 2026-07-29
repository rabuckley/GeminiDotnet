using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Structured representation of a function declaration as defined by the
/// [OpenAPI 3.03 specification](https://spec.openapis.org/oas/v3.0.3). Included
/// in this declaration are the function name and parameters. This
/// FunctionDeclaration is a representation of a block of code that can be used
/// as a <see cref="V1.Tool"/> by the model and executed by the client.
/// </summary>
public sealed record FunctionDeclaration
{
    /// <summary>
    /// Required. A brief description of the function.
    /// </summary>
    [JsonPropertyName("description")]
    public required string Description { get; init; }

    /// <summary>
    /// Required. The name of the function.
    /// Must be a-z, A-Z, 0-9, or contain underscores, colons, dots, and dashes,
    /// with a maximum length of 128.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Optional. Describes the parameters to this function. Reflects the Open API 3.03
    /// Parameter Object string Key: the name of the parameter. Parameter names are
    /// case sensitive. Schema Value: the Schema defining the type used for the
    /// parameter.
    /// </summary>
    [JsonPropertyName("parameters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Schema? Parameters { get; init; }

    /// <summary>
    /// Optional. Describes the output from this function in JSON Schema format. Reflects the
    /// Open API 3.03 Response Object. The Schema defines the type used for the
    /// response value of the function.
    /// </summary>
    [JsonPropertyName("response")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Schema? Response { get; init; }
}

