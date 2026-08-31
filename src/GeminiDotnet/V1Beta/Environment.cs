using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// An execution environment for an agent.
/// </summary>
public sealed record Environment
{
    /// <summary>
    /// Output only. The time at which the environment was created in ISO 8601 format
    /// (YYYY-MM-DDThh:mm:ssZ).
    /// </summary>
    [JsonPropertyName("created")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Created { get; init; }

    /// <summary>
    /// Output only. The number of files in the environment, output only.
    /// </summary>
    [JsonPropertyName("fileCount")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? FileCount { get; init; }

    /// <summary>
    /// Required. Output only. The ID of the environment.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    /// <summary>
    /// Output only. The time at which the environment was last accessed in ISO 8601 format
    /// (YYYY-MM-DDThh:mm:ssZ).
    /// </summary>
    [JsonPropertyName("lastAccessed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? LastAccessed { get; init; }

    /// <summary>
    /// Allow only specific domains.
    /// </summary>
    [JsonPropertyName("networkAllowlist")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public EnvironmentNetworkEgressAllowlist? NetworkAllowlist { get; init; }

    /// <summary>
    /// Network egress mode.
    /// </summary>
    [JsonPropertyName("networkMode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public EnvironmentNetworkMode? NetworkMode { get; init; }

    /// <summary>
    /// Output only. The total size of the environment files in bytes, output only.
    /// </summary>
    [JsonPropertyName("sizeBytes")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? SizeBytes { get; init; }

    /// <summary>
    /// Sources to be mounted into the environment.
    /// </summary>
    [JsonPropertyName("sources")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Source>? Sources { get; init; }

    /// <summary>
    /// Output only. The status of the environment container.
    /// </summary>
    [JsonPropertyName("status")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public EnvironmentStatus? Status { get; init; }

    /// <summary>
    /// Output only. The time at which the environment was last updated in ISO 8601 format
    /// (YYYY-MM-DDThh:mm:ssZ).
    /// </summary>
    [JsonPropertyName("updated")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Updated { get; init; }
}

