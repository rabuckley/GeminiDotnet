using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Optional. The MIME type of the text output.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(TextResponseFormatMimeType.Converter))]
public readonly struct TextResponseFormatMimeType : IEquatable<TextResponseFormatMimeType>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextResponseFormatMimeType"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public TextResponseFormatMimeType(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Default value. This value is unused.
    /// </summary>
    public static TextResponseFormatMimeType Unspecified { get; } = new("MIME_TYPE_UNSPECIFIED");

    /// <summary>
    /// JSON output format.
    /// </summary>
    public static TextResponseFormatMimeType ApplicationJson { get; } = new("APPLICATION_JSON");

    /// <summary>
    /// Plain text output format.
    /// </summary>
    public static TextResponseFormatMimeType TextPlain { get; } = new("TEXT_PLAIN");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<TextResponseFormatMimeType> KnownValues { get; } =
    [
        Unspecified,
        ApplicationJson,
        TextPlain,
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
    public static bool operator ==(TextResponseFormatMimeType left, TextResponseFormatMimeType right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(TextResponseFormatMimeType left, TextResponseFormatMimeType right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(TextResponseFormatMimeType other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is TextResponseFormatMimeType other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<TextResponseFormatMimeType>
    {
        /// <inheritdoc/>
        public override TextResponseFormatMimeType Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for TextResponseFormatMimeType."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, TextResponseFormatMimeType value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

