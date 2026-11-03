using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. Configures transcription mode. Supported values: <c>VERBATIM</c>,
/// <c>SMART</c>. If unspecified, defaults to <c>VERBATIM</c> transcription.
/// In <c>SMART</c> mode, the model performs disfluency removal (eliminating
/// filler words, repetitions, and false starts), light grammatical cleanup,
/// automatic formatting (paragraphs, bullet points, numbered lists), and
/// minor user edits (inline self-corrections).
/// Timestamps and diarization are incompatible with mode <c>SMART</c>.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(AudioTranscriptionConfigMode.Converter))]
public readonly struct AudioTranscriptionConfigMode : IEquatable<AudioTranscriptionConfigMode>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioTranscriptionConfigMode"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public AudioTranscriptionConfigMode(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Unspecified transcription mode.
    /// </summary>
    public static AudioTranscriptionConfigMode Unspecified { get; } = new("MODE_UNSPECIFIED");

    /// <summary>
    /// Verbatim transcription mode.
    /// </summary>
    public static AudioTranscriptionConfigMode Verbatim { get; } = new("VERBATIM");

    /// <summary>
    /// Smart transcription mode.
    /// </summary>
    public static AudioTranscriptionConfigMode Smart { get; } = new("SMART");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<AudioTranscriptionConfigMode> KnownValues { get; } =
    [
        Unspecified,
        Verbatim,
        Smart,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "MODE_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(AudioTranscriptionConfigMode left, AudioTranscriptionConfigMode right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(AudioTranscriptionConfigMode left, AudioTranscriptionConfigMode right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(AudioTranscriptionConfigMode other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is AudioTranscriptionConfigMode other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<AudioTranscriptionConfigMode>
    {
        /// <inheritdoc/>
        public override AudioTranscriptionConfigMode Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for AudioTranscriptionConfigMode."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, AudioTranscriptionConfigMode value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

