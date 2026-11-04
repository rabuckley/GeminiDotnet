using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(ServiceTier.Converter))]
public readonly struct ServiceTier : IEquatable<ServiceTier>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceTier"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public ServiceTier(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Default service tier, which is standard.
    /// </summary>
    public static ServiceTier Unspecified { get; } = new("unspecified");

    /// <summary>
    /// Standard service tier.
    /// </summary>
    public static ServiceTier Standard { get; } = new("standard");

    /// <summary>
    /// Flex service tier.
    /// </summary>
    public static ServiceTier Flex { get; } = new("flex");

    /// <summary>
    /// Priority service tier.
    /// </summary>
    public static ServiceTier Priority { get; } = new("priority");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<ServiceTier> KnownValues { get; } =
    [
        Unspecified,
        Standard,
        Flex,
        Priority,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "unspecified";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(ServiceTier left, ServiceTier right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(ServiceTier left, ServiceTier right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(ServiceTier other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is ServiceTier other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<ServiceTier>
    {
        /// <inheritdoc/>
        public override ServiceTier Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for ServiceTier."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ServiceTier value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

