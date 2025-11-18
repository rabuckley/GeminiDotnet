using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.TunedModels;

/// <summary>
/// Request to transfer the ownership of the tuned model.
/// </summary>
public sealed record TransferOwnershipRequest
{
    /// <summary>
    /// Required. The email address of the user to whom the tuned model is being transferred
    /// to.
    /// </summary>
    [JsonPropertyName("emailAddress")]
    public required string EmailAddress { get; init; }
}

