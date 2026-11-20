using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. Controls text normalization before speech synthesis.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(SpeechConfigTextNormalization.Converter))]
public readonly struct SpeechConfigTextNormalization : IEquatable<SpeechConfigTextNormalization>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpeechConfigTextNormalization"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public SpeechConfigTextNormalization(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Unspecified text normalization mode. Defaults to <c>AUTO</c>.
    /// </summary>
    public static SpeechConfigTextNormalization Unspecified { get; } = new("TEXT_NORMALIZATION_UNSPECIFIED");

    /// <summary>
    /// Automatically determines whether to apply text normalization based on
    /// the model and speech mode.
    /// </summary>
    public static SpeechConfigTextNormalization Auto { get; } = new("AUTO");

    /// <summary>
    /// Enables text normalization for numbers, currencies, dates, and units.
    /// </summary>
    public static SpeechConfigTextNormalization Enabled { get; } = new("ENABLED");

    /// <summary>
    /// Disables text normalization for pre-normalized or phonetic inputs.
    /// </summary>
    public static SpeechConfigTextNormalization Disabled { get; } = new("DISABLED");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<SpeechConfigTextNormalization> KnownValues { get; } =
    [
        Unspecified,
        Auto,
        Enabled,
        Disabled,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "TEXT_NORMALIZATION_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(SpeechConfigTextNormalization left, SpeechConfigTextNormalization right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(SpeechConfigTextNormalization left, SpeechConfigTextNormalization right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(SpeechConfigTextNormalization other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is SpeechConfigTextNormalization other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<SpeechConfigTextNormalization>
    {
        /// <inheritdoc/>
        public override SpeechConfigTextNormalization Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for SpeechConfigTextNormalization."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, SpeechConfigTextNormalization value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

