using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Environments;

/// <summary>
/// An environment variable to set in the execution environment.
/// </summary>
public sealed record EnvVar
{
    /// <summary>
    /// Optional reference to a server-managed Credential resource by ID.
    /// </summary>
    [JsonPropertyName("credential")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Credential { get; init; }

    /// <summary>
    /// Direct string value for plain environment variables.
    /// </summary>
    [JsonPropertyName("value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Value { get; init; }
}

