using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Optional. If set, the prompt was blocked and no candidates are returned.
/// Rephrase the prompt.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(PromptFeedbackBlockReason.Converter))]
public readonly struct PromptFeedbackBlockReason : IEquatable<PromptFeedbackBlockReason>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="PromptFeedbackBlockReason"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public PromptFeedbackBlockReason(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Default value. This value is unused.
    /// </summary>
    public static PromptFeedbackBlockReason Unspecified { get; } = new("BLOCK_REASON_UNSPECIFIED");

    /// <summary>
    /// Prompt was blocked due to safety reasons. Inspect <c>safety_ratings</c>
    /// to understand which safety category blocked it.
    /// </summary>
    public static PromptFeedbackBlockReason Safety { get; } = new("SAFETY");

    /// <summary>
    /// Prompt was blocked due to unknown reasons.
    /// </summary>
    public static PromptFeedbackBlockReason Other { get; } = new("OTHER");

    /// <summary>
    /// Prompt was blocked due to the terms which are included from the
    /// terminology blocklist.
    /// </summary>
    public static PromptFeedbackBlockReason Blocklist { get; } = new("BLOCKLIST");

    /// <summary>
    /// Prompt was blocked due to prohibited content.
    /// </summary>
    public static PromptFeedbackBlockReason ProhibitedContent { get; } = new("PROHIBITED_CONTENT");

    /// <summary>
    /// Candidates blocked due to unsafe image generation content.
    /// </summary>
    public static PromptFeedbackBlockReason ImageSafety { get; } = new("IMAGE_SAFETY");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<PromptFeedbackBlockReason> KnownValues { get; } =
    [
        Unspecified,
        Safety,
        Other,
        Blocklist,
        ProhibitedContent,
        ImageSafety,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "BLOCK_REASON_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(PromptFeedbackBlockReason left, PromptFeedbackBlockReason right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(PromptFeedbackBlockReason left, PromptFeedbackBlockReason right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(PromptFeedbackBlockReason other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is PromptFeedbackBlockReason other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<PromptFeedbackBlockReason>
    {
        /// <inheritdoc/>
        public override PromptFeedbackBlockReason Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for PromptFeedbackBlockReason."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, PromptFeedbackBlockReason value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

