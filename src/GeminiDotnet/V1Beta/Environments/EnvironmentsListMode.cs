using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Environments;

/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(EnvironmentsListMode.Converter))]
public readonly struct EnvironmentsListMode : IEquatable<EnvironmentsListMode>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnvironmentsListMode"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public EnvironmentsListMode(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    public static EnvironmentsListMode EnvironmentModeUnspecified { get; } = new("ENVIRONMENT_MODE_UNSPECIFIED");

    public static EnvironmentsListMode OnDemand { get; } = new("ON_DEMAND");

    public static EnvironmentsListMode SelfHosted { get; } = new("SELF_HOSTED");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<EnvironmentsListMode> KnownValues { get; } =
    [
        EnvironmentModeUnspecified,
        OnDemand,
        SelfHosted,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="EnvironmentModeUnspecified"/>.
    /// </value>
    public string Value => _value ?? "ENVIRONMENT_MODE_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(EnvironmentsListMode left, EnvironmentsListMode right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(EnvironmentsListMode left, EnvironmentsListMode right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(EnvironmentsListMode other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is EnvironmentsListMode other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<EnvironmentsListMode>
    {
        /// <inheritdoc/>
        public override EnvironmentsListMode Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for EnvironmentsListMode."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, EnvironmentsListMode value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

