using System.Text.Json.Serialization;

namespace GeminiDotnet.V1.FileSearchStores;

/// <summary>
/// Detailed Content-Type information from Scotty. The Content-Type of the media
/// will typically be filled in by the header or Scotty's best_guess, but this
/// extended information provides the backend with more information so that it
/// can make a better decision if needed. This is only used on media upload
/// requests from Scotty.
/// </summary>
public sealed record ContentTypeInfo
{
    /// <summary>
    /// Scotty's best guess of what the content type of the file is.
    /// </summary>
    [JsonPropertyName("bestGuess")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? BestGuess { get; init; }

    /// <summary>
    /// The content type of the file derived by looking at specific
    /// bytes (i.e. "magic bytes") of the actual file.
    /// </summary>
    [JsonPropertyName("fromBytes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? FromBytes { get; init; }

    /// <summary>
    /// The content type of the file derived from the file extension of
    /// the original file name used by the client.
    /// </summary>
    [JsonPropertyName("fromFileName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? FromFileName { get; init; }

    /// <summary>
    /// The content type of the file detected by Fusion ID. go/fusionid
    /// </summary>
    [JsonPropertyName("fromFusionId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? FromFusionId { get; init; }

    /// <summary>
    /// The content type of the file as specified in the request headers,
    /// multipart headers, or RUPIO start request.
    /// </summary>
    [JsonPropertyName("fromHeader")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? FromHeader { get; init; }

    /// <summary>
    /// The content type of the file derived from the file extension of the
    /// URL path.  The URL path is assumed to represent a file name (which
    /// is typically only true for agents that are providing a REST API).
    /// </summary>
    [JsonPropertyName("fromUrlPath")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? FromUrlPath { get; init; }

    /// <summary>
    /// Metadata information from Fusion ID detection. Serialized
    /// FusionIdDetectionMetadata proto. Only set if from_fusion_id is set.
    /// </summary>
    [JsonPropertyName("fusionIdDetectionMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<byte> FusionIdDetectionMetadata { get; init; }
}

