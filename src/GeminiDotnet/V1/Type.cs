using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(Type.Converter))]
public readonly struct Type : IEquatable<Type>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Type"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public Type(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Not specified, should not be used.
    /// </summary>
    public static Type Unspecified { get; } = new("TYPE_UNSPECIFIED");

    /// <summary>
    /// String type.
    /// </summary>
    public static Type String { get; } = new("STRING");

    /// <summary>
    /// Number type.
    /// </summary>
    public static Type Number { get; } = new("NUMBER");

    /// <summary>
    /// Integer type.
    /// </summary>
    public static Type Integer { get; } = new("INTEGER");

    /// <summary>
    /// Boolean type.
    /// </summary>
    public static Type Boolean { get; } = new("BOOLEAN");

    /// <summary>
    /// Array type.
    /// </summary>
    public static Type Array { get; } = new("ARRAY");

    /// <summary>
    /// Object type.
    /// </summary>
    public static Type Object { get; } = new("OBJECT");

    /// <summary>
    /// Null type.
    /// </summary>
    public static Type Null { get; } = new("NULL");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<Type> KnownValues { get; } =
    [
        Unspecified,
        String,
        Number,
        Integer,
        Boolean,
        Array,
        Object,
        Null,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "TYPE_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(Type left, Type right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(Type left, Type right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(Type other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Type other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<Type>
    {
        /// <inheritdoc/>
        public override Type Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for Type."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

