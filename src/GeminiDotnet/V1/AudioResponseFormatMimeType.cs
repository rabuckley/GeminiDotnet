using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Optional. The MIME type of the audio output.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(AudioResponseFormatMimeType.Converter))]
public readonly struct AudioResponseFormatMimeType : IEquatable<AudioResponseFormatMimeType>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioResponseFormatMimeType"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public AudioResponseFormatMimeType(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Default value. This value is unused.
    /// </summary>
    public static AudioResponseFormatMimeType Unspecified { get; } = new("MIME_TYPE_UNSPECIFIED");

    /// <summary>
    /// MP3 audio format.
    /// </summary>
    public static AudioResponseFormatMimeType AudioMp3 { get; } = new("AUDIO_MP3");

    /// <summary>
    /// OGG Opus audio format.
    /// </summary>
    public static AudioResponseFormatMimeType AudioOggOpus { get; } = new("AUDIO_OGG_OPUS");

    /// <summary>
    /// Raw PCM (L16) audio format.
    /// </summary>
    public static AudioResponseFormatMimeType AudioL16 { get; } = new("AUDIO_L16");

    /// <summary>
    /// WAV audio format.
    /// </summary>
    public static AudioResponseFormatMimeType AudioWav { get; } = new("AUDIO_WAV");

    /// <summary>
    /// A-law audio format.
    /// </summary>
    public static AudioResponseFormatMimeType AudioAlaw { get; } = new("AUDIO_ALAW");

    /// <summary>
    /// Mu-law audio format.
    /// </summary>
    public static AudioResponseFormatMimeType AudioMulaw { get; } = new("AUDIO_MULAW");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<AudioResponseFormatMimeType> KnownValues { get; } =
    [
        Unspecified,
        AudioMp3,
        AudioOggOpus,
        AudioL16,
        AudioWav,
        AudioAlaw,
        AudioMulaw,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "MIME_TYPE_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(AudioResponseFormatMimeType left, AudioResponseFormatMimeType right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(AudioResponseFormatMimeType left, AudioResponseFormatMimeType right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(AudioResponseFormatMimeType other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is AudioResponseFormatMimeType other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<AudioResponseFormatMimeType>
    {
        /// <inheritdoc/>
        public override AudioResponseFormatMimeType Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for AudioResponseFormatMimeType."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, AudioResponseFormatMimeType value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

