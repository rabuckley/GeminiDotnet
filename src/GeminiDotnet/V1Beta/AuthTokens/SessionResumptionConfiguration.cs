using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.AuthTokens;

/// <summary>
/// Session resumption configuration.
/// This message is included in the session configuration as
/// <c>BidiGenerateContentSetup.session_resumption</c>. If configured, the server
/// will send <c>SessionResumptionUpdate</c> messages.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record SessionResumptionConfiguration
{
    /// <summary>
    /// The handle of a previous session. If not present then a new session is
    /// created.
    /// Session handles come from <c>SessionResumptionUpdate.token</c> values in
    /// previous connections.
    /// </summary>
    [JsonPropertyName("handle")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Handle { get; init; }
}

