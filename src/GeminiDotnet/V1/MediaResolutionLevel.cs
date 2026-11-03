using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// The tokenization quality used for given media.
/// for Gemini API support .
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(MediaResolutionLevel.Converter))]
public readonly struct MediaResolutionLevel : IEquatable<MediaResolutionLevel>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="MediaResolutionLevel"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public MediaResolutionLevel(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Media resolution has not been set.
    /// </summary>
    public static MediaResolutionLevel MediaResolutionUnspecified { get; } = new("MEDIA_RESOLUTION_UNSPECIFIED");

    /// <summary>
    /// Media resolution set to low.
    /// </summary>
    public static MediaResolutionLevel MediaResolutionLow { get; } = new("MEDIA_RESOLUTION_LOW");

    /// <summary>
    /// Media resolution set to medium.
    /// </summary>
    public static MediaResolutionLevel MediaResolutionMedium { get; } = new("MEDIA_RESOLUTION_MEDIUM");

    /// <summary>
    /// Media resolution set to high.
    /// </summary>
    public static MediaResolutionLevel MediaResolutionHigh { get; } = new("MEDIA_RESOLUTION_HIGH");

    /// <summary>
    /// Media resolution set to ultra high.
    /// </summary>
    public static MediaResolutionLevel MediaResolutionUltraHigh { get; } = new("MEDIA_RESOLUTION_ULTRA_HIGH");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<MediaResolutionLevel> KnownValues { get; } =
    [
        MediaResolutionUnspecified,
        MediaResolutionLow,
        MediaResolutionMedium,
        MediaResolutionHigh,
        MediaResolutionUltraHigh,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="MediaResolutionUnspecified"/>.
    /// </value>
    public string Value => _value ?? "MEDIA_RESOLUTION_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(MediaResolutionLevel left, MediaResolutionLevel right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(MediaResolutionLevel left, MediaResolutionLevel right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(MediaResolutionLevel other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is MediaResolutionLevel other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<MediaResolutionLevel>
    {
        /// <inheritdoc/>
        public override MediaResolutionLevel Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for MediaResolutionLevel."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, MediaResolutionLevel value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

