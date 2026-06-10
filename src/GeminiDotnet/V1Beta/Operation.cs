using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// This resource represents a long-running operation that is the result of a
/// network API call.
/// </summary>
public sealed record Operation : BaseOperation
{
    /// <summary>
    /// Service-specific metadata associated with the operation.  It typically
    /// contains progress information and common metadata such as create time.
    /// Some services might not provide such metadata.  Any method that returns a
    /// long-running operation should document the metadata type, if any.
    /// </summary>
    [JsonPropertyName("metadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JsonElement Metadata { get; init; }

    /// <summary>
    /// The normal, successful response of the operation.  If the original
    /// method returns no data on success, such as <c>Delete</c>, the response is
    /// <c>google.protobuf.Empty</c>.  If the original method is standard
    /// <c>Get</c>/<c>Create</c>/<c>Update</c>, the response should be the resource.  For other
    /// methods, the response should have the type <c>XxxResponse</c>, where <c>Xxx</c>
    /// is the original method name.  For example, if the original method name
    /// is <c>TakeSnapshot()</c>, the inferred response type is
    /// <c>TakeSnapshotResponse</c>.
    /// </summary>
    [JsonPropertyName("response")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JsonElement Response { get; init; }
}

