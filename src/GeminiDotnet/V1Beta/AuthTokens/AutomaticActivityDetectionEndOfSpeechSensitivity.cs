using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.AuthTokens;

/// <summary>
/// Optional. Determines how likely detected speech is ended.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(AutomaticActivityDetectionEndOfSpeechSensitivity.Converter))]
public readonly struct AutomaticActivityDetectionEndOfSpeechSensitivity : IEquatable<AutomaticActivityDetectionEndOfSpeechSensitivity>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="AutomaticActivityDetectionEndOfSpeechSensitivity"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public AutomaticActivityDetectionEndOfSpeechSensitivity(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// The default is END_SENSITIVITY_HIGH.
    /// </summary>
    public static AutomaticActivityDetectionEndOfSpeechSensitivity EndSensitivityUnspecified { get; } = new("END_SENSITIVITY_UNSPECIFIED");

    /// <summary>
    /// Automatic detection ends speech more often.
    /// </summary>
    public static AutomaticActivityDetectionEndOfSpeechSensitivity EndSensitivityHigh { get; } = new("END_SENSITIVITY_HIGH");

    /// <summary>
    /// Automatic detection ends speech less often.
    /// </summary>
    public static AutomaticActivityDetectionEndOfSpeechSensitivity EndSensitivityLow { get; } = new("END_SENSITIVITY_LOW");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<AutomaticActivityDetectionEndOfSpeechSensitivity> KnownValues { get; } =
    [
        EndSensitivityUnspecified,
        EndSensitivityHigh,
        EndSensitivityLow,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="EndSensitivityUnspecified"/>.
    /// </value>
    public string Value => _value ?? "END_SENSITIVITY_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(AutomaticActivityDetectionEndOfSpeechSensitivity left, AutomaticActivityDetectionEndOfSpeechSensitivity right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(AutomaticActivityDetectionEndOfSpeechSensitivity left, AutomaticActivityDetectionEndOfSpeechSensitivity right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(AutomaticActivityDetectionEndOfSpeechSensitivity other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is AutomaticActivityDetectionEndOfSpeechSensitivity other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<AutomaticActivityDetectionEndOfSpeechSensitivity>
    {
        /// <inheritdoc/>
        public override AutomaticActivityDetectionEndOfSpeechSensitivity Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for AutomaticActivityDetectionEndOfSpeechSensitivity."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, AutomaticActivityDetectionEndOfSpeechSensitivity value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

