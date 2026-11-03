using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Optional. The requested modalities of the response. Represents the set of modalities
/// that the model can return, and should be expected in the response. This is
/// an exact match to the modalities of the response.
/// A model may have multiple combinations of supported modalities. If the
/// requested modalities do not match any of the supported combinations, an
/// error will be returned.
/// An empty list is equivalent to requesting only text.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(ResponseModality.Converter))]
public readonly struct ResponseModality : IEquatable<ResponseModality>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResponseModality"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public ResponseModality(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    public static ResponseModality Unspecified { get; } = new("MODALITY_UNSPECIFIED");

    public static ResponseModality Text { get; } = new("TEXT");

    public static ResponseModality Image { get; } = new("IMAGE");

    public static ResponseModality Audio { get; } = new("AUDIO");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<ResponseModality> KnownValues { get; } =
    [
        Unspecified,
        Text,
        Image,
        Audio,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "MODALITY_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(ResponseModality left, ResponseModality right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(ResponseModality left, ResponseModality right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(ResponseModality other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is ResponseModality other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<ResponseModality>
    {
        /// <inheritdoc/>
        public override ResponseModality Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for ResponseModality."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ResponseModality value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

