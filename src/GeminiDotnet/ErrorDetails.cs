using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet;

public sealed record ErrorDetails
{
    [JsonPropertyName("code")]
    public required HttpStatusCode StatusCode { get; init; }

    [JsonPropertyName("message")]
    public required string Message { get; init; }

    [JsonPropertyName("status")]
    public required string Status { get; init; }

    [JsonPropertyName("details")]
    public JsonElement Details { get; init; }
}
