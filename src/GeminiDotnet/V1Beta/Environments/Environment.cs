using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Environments;

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
    /// Output only. Arbitrary customer-defined JSON configuration passed through to the
    /// self-hosted environment poller. Only populated for <c>self_hosted</c>
    /// environments when configured.
    /// </summary>
    [JsonPropertyName("customConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JsonElement CustomConfiguration { get; init; }

    /// <summary>
    /// Output only. Secret token generated upon environment creation for self_hosted
    /// environments. Used by the self-hosted sandbox daemon to authenticate
    /// polling and streaming requests. Only returned in the CreateEnvironment
    /// response; omitted from subsequent GetEnvironment and ListEnvironments
    /// calls.
    /// </summary>
    [JsonPropertyName("environmentKey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? EnvironmentKey { get; init; }

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
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Id { get; init; }

    /// <summary>
    /// Output only. The time at which the environment was last accessed in ISO 8601 format
    /// (YYYY-MM-DDThh:mm:ssZ).
    /// </summary>
    [JsonPropertyName("lastAccessed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? LastAccessed { get; init; }

    /// <summary>
    /// Output only. The compute lifecycle mode for this environment.
    /// </summary>
    [JsonPropertyName("mode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public EnvironmentMode? Mode { get; init; }

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

