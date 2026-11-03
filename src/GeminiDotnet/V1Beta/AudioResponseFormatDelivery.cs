using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. The delivery mode for the audio output.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(AudioResponseFormatDelivery.Converter))]
public readonly struct AudioResponseFormatDelivery : IEquatable<AudioResponseFormatDelivery>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioResponseFormatDelivery"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public AudioResponseFormatDelivery(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Default value. This value is unused.
    /// </summary>
    public static AudioResponseFormatDelivery Unspecified { get; } = new("DELIVERY_UNSPECIFIED");

    /// <summary>
    /// Audio data is returned inline in the response.
    /// </summary>
    public static AudioResponseFormatDelivery Inline { get; } = new("INLINE");

    /// <summary>
    /// Audio data is returned as a URI.
    /// </summary>
    public static AudioResponseFormatDelivery Uri { get; } = new("URI");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<AudioResponseFormatDelivery> KnownValues { get; } =
    [
        Unspecified,
        Inline,
        Uri,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "DELIVERY_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(AudioResponseFormatDelivery left, AudioResponseFormatDelivery right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(AudioResponseFormatDelivery left, AudioResponseFormatDelivery right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(AudioResponseFormatDelivery other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is AudioResponseFormatDelivery other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<AudioResponseFormatDelivery>
    {
        /// <inheritdoc/>
        public override AudioResponseFormatDelivery Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for AudioResponseFormatDelivery."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, AudioResponseFormatDelivery value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

