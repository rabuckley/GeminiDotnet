using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.AuthTokens;

/// <summary>
/// History configuration.
/// This message is included in the session configuration as
/// <c>BidiGenerateContentSetup.history_config</c>. Configures the exchange of
/// history messages.
/// </summary>
public sealed record HistoryConfiguration
{
    /// <summary>
    /// Optional. If true, after sending <c>setup_complete</c>, the server will wait
    /// and at first process <c>client_content</c> messages until <c>turn_complete</c> is
    /// <c>true</c>. This initial history will not trigger a model call and
    /// may end with role <c>MODEL</c>. After <c>turn_complete</c> is <c>true</c>, the client
    /// can start the realtime conversation via <c>realtime_input</c>.
    /// </summary>
    [JsonPropertyName("initialHistoryInClientContent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? InitialHistoryInClientContent { get; init; }
}

