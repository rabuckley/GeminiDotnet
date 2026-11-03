using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// The mode of the predictor to be used in dynamic retrieval.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(DynamicRetrievalConfigMode.Converter))]
public readonly struct DynamicRetrievalConfigMode : IEquatable<DynamicRetrievalConfigMode>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicRetrievalConfigMode"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public DynamicRetrievalConfigMode(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Always trigger retrieval.
    /// </summary>
    public static DynamicRetrievalConfigMode Unspecified { get; } = new("MODE_UNSPECIFIED");

    /// <summary>
    /// Run retrieval only when system decides it is necessary.
    /// </summary>
    public static DynamicRetrievalConfigMode Dynamic { get; } = new("MODE_DYNAMIC");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<DynamicRetrievalConfigMode> KnownValues { get; } =
    [
        Unspecified,
        Dynamic,
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
    public static bool operator ==(DynamicRetrievalConfigMode left, DynamicRetrievalConfigMode right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(DynamicRetrievalConfigMode left, DynamicRetrievalConfigMode right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(DynamicRetrievalConfigMode other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is DynamicRetrievalConfigMode other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<DynamicRetrievalConfigMode>
    {
        /// <inheritdoc/>
        public override DynamicRetrievalConfigMode Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for DynamicRetrievalConfigMode."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, DynamicRetrievalConfigMode value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

