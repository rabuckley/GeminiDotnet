using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Optional. If set, the input was blocked and no candidates are returned.
/// Rephrase the input.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(InputFeedbackBlockReason.Converter))]
public readonly struct InputFeedbackBlockReason : IEquatable<InputFeedbackBlockReason>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="InputFeedbackBlockReason"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public InputFeedbackBlockReason(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Default value. This value is unused.
    /// </summary>
    public static InputFeedbackBlockReason Unspecified { get; } = new("BLOCK_REASON_UNSPECIFIED");

    /// <summary>
    /// Input was blocked due to safety reasons. Inspect
    /// <c>safety_ratings</c> to understand which safety category blocked it.
    /// </summary>
    public static InputFeedbackBlockReason Safety { get; } = new("SAFETY");

    /// <summary>
    /// Input was blocked due to other reasons.
    /// </summary>
    public static InputFeedbackBlockReason Other { get; } = new("OTHER");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<InputFeedbackBlockReason> KnownValues { get; } =
    [
        Unspecified,
        Safety,
        Other,
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
    public static bool operator ==(InputFeedbackBlockReason left, InputFeedbackBlockReason right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(InputFeedbackBlockReason left, InputFeedbackBlockReason right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(InputFeedbackBlockReason other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is InputFeedbackBlockReason other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<InputFeedbackBlockReason>
    {
        /// <inheritdoc/>
        public override InputFeedbackBlockReason Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for InputFeedbackBlockReason."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, InputFeedbackBlockReason value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

