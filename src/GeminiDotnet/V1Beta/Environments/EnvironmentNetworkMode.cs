using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Environments;

/// <summary>
/// Network egress mode.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(EnvironmentNetworkMode.Converter))]
public readonly struct EnvironmentNetworkMode : IEquatable<EnvironmentNetworkMode>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnvironmentNetworkMode"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public EnvironmentNetworkMode(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Default value. Unused.
    /// </summary>
    public static EnvironmentNetworkMode Unspecified { get; } = new("NETWORK_MODE_UNSPECIFIED");

    /// <summary>
    /// All network egress is blocked.
    /// </summary>
    public static EnvironmentNetworkMode Disabled { get; } = new("DISABLED");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<EnvironmentNetworkMode> KnownValues { get; } =
    [
        Unspecified,
        Disabled,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "NETWORK_MODE_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(EnvironmentNetworkMode left, EnvironmentNetworkMode right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(EnvironmentNetworkMode left, EnvironmentNetworkMode right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(EnvironmentNetworkMode other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is EnvironmentNetworkMode other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<EnvironmentNetworkMode>
    {
        /// <inheritdoc/>
        public override EnvironmentNetworkMode Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for EnvironmentNetworkMode."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, EnvironmentNetworkMode value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

