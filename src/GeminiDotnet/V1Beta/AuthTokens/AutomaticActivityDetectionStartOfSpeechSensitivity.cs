using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.AuthTokens;

/// <summary>
/// Optional. Determines how likely speech is to be detected.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(AutomaticActivityDetectionStartOfSpeechSensitivity.Converter))]
public readonly struct AutomaticActivityDetectionStartOfSpeechSensitivity : IEquatable<AutomaticActivityDetectionStartOfSpeechSensitivity>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="AutomaticActivityDetectionStartOfSpeechSensitivity"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public AutomaticActivityDetectionStartOfSpeechSensitivity(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// The default is START_SENSITIVITY_HIGH.
    /// </summary>
    public static AutomaticActivityDetectionStartOfSpeechSensitivity StartSensitivityUnspecified { get; } = new("START_SENSITIVITY_UNSPECIFIED");

    /// <summary>
    /// Automatic detection will detect the start of speech more often.
    /// </summary>
    public static AutomaticActivityDetectionStartOfSpeechSensitivity StartSensitivityHigh { get; } = new("START_SENSITIVITY_HIGH");

    /// <summary>
    /// Automatic detection will detect the start of speech less often.
    /// </summary>
    public static AutomaticActivityDetectionStartOfSpeechSensitivity StartSensitivityLow { get; } = new("START_SENSITIVITY_LOW");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<AutomaticActivityDetectionStartOfSpeechSensitivity> KnownValues { get; } =
    [
        StartSensitivityUnspecified,
        StartSensitivityHigh,
        StartSensitivityLow,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="StartSensitivityUnspecified"/>.
    /// </value>
    public string Value => _value ?? "START_SENSITIVITY_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(AutomaticActivityDetectionStartOfSpeechSensitivity left, AutomaticActivityDetectionStartOfSpeechSensitivity right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(AutomaticActivityDetectionStartOfSpeechSensitivity left, AutomaticActivityDetectionStartOfSpeechSensitivity right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(AutomaticActivityDetectionStartOfSpeechSensitivity other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is AutomaticActivityDetectionStartOfSpeechSensitivity other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<AutomaticActivityDetectionStartOfSpeechSensitivity>
    {
        /// <inheritdoc/>
        public override AutomaticActivityDetectionStartOfSpeechSensitivity Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for AutomaticActivityDetectionStartOfSpeechSensitivity."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, AutomaticActivityDetectionStartOfSpeechSensitivity value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

