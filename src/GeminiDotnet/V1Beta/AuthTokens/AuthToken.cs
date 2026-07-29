using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.AuthTokens;

/// <summary>
/// A request to create an ephemeral authentication token.
/// </summary>
public sealed record AuthToken
{
    /// <summary>
    /// Optional. Input only. Immutable. Configuration specific to <c>BidiGenerateContent</c>.
    /// </summary>
    [JsonPropertyName("bidiGenerateContentSetup")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public BidiGenerateContentSetup? BidiGenerateContentSetup { get; init; }

    /// <summary>
    /// Optional. Input only. Immutable. An optional time after which, when using the resulting token, messages in
    /// BidiGenerateContent sessions will be rejected. (Gemini may preemptively
    /// close the session after this time.)
    /// If not set then this defaults to 30 minutes in the future. If set, this
    /// value must be less than 20 hours in the future.
    /// </summary>
    [JsonPropertyName("expireTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? ExpireTime { get; init; }

    /// <summary>
    /// Optional. Input only. Immutable. If field_mask is empty, and <c>bidi_generate_content_setup</c> is not present,
    /// then the effective <see cref="V1Beta.AuthTokens.BidiGenerateContentSetup"/> message is taken from the
    /// Live API connection.
    /// If field_mask is empty, and <c>bidi_generate_content_setup</c> _is_ present,
    /// then the effective <see cref="V1Beta.AuthTokens.BidiGenerateContentSetup"/> message is taken entirely
    /// from <c>bidi_generate_content_setup</c> in this request. The setup message from
    /// the Live API connection is ignored.
    /// If field_mask is not empty, then the corresponding fields from
    /// <c>bidi_generate_content_setup</c> will overwrite the fields from the setup
    /// message in the Live API connection.
    /// </summary>
    [JsonPropertyName("fieldMask")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? FieldMask { get; init; }

    /// <summary>
    /// Optional. Input only. Immutable. The interaction ID that this token is scoped to.
    /// Specific to the Live Interactions API.
    /// </summary>
    [JsonPropertyName("interactionId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? InteractionId { get; init; }

    /// <summary>
    /// Output only. Identifier. The token itself.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Name { get; init; }

    /// <summary>
    /// Optional. Input only. Immutable. The time after which new Live API sessions using the token resulting from
    /// this request will be rejected.
    /// If not set this defaults to 60 seconds in the future. If set, this value
    /// must be less than 20 hours in the future.
    /// </summary>
    [JsonPropertyName("newSessionExpireTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? NewSessionExpireTime { get; init; }

    /// <summary>
    /// Optional. Input only. Immutable. The number of times the token can be used. If this value is zero then no
    /// limit is applied. Resuming a Live API session does not count as a use. If
    /// unspecified, the default is 1.
    /// </summary>
    [JsonPropertyName("uses")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? Uses { get; init; }
}

