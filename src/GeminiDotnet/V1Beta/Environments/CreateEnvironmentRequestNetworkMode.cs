using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Environments;

/// <summary>
/// Network egress mode.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(CreateEnvironmentRequestNetworkMode.Converter))]
public readonly struct CreateEnvironmentRequestNetworkMode : IEquatable<CreateEnvironmentRequestNetworkMode>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateEnvironmentRequestNetworkMode"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public CreateEnvironmentRequestNetworkMode(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Default value. Unused.
    /// </summary>
    public static CreateEnvironmentRequestNetworkMode Unspecified { get; } = new("NETWORK_MODE_UNSPECIFIED");

    /// <summary>
    /// All network egress is blocked.
    /// </summary>
    public static CreateEnvironmentRequestNetworkMode Disabled { get; } = new("DISABLED");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<CreateEnvironmentRequestNetworkMode> KnownValues { get; } =
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
    public static bool operator ==(CreateEnvironmentRequestNetworkMode left, CreateEnvironmentRequestNetworkMode right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(CreateEnvironmentRequestNetworkMode left, CreateEnvironmentRequestNetworkMode right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(CreateEnvironmentRequestNetworkMode other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is CreateEnvironmentRequestNetworkMode other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<CreateEnvironmentRequestNetworkMode>
    {
        /// <inheritdoc/>
        public override CreateEnvironmentRequestNetworkMode Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for CreateEnvironmentRequestNetworkMode."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, CreateEnvironmentRequestNetworkMode value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

