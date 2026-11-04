using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Environments;

/// <summary>
/// Output only. The status of the environment container.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(EnvironmentStatus.Converter))]
public readonly struct EnvironmentStatus : IEquatable<EnvironmentStatus>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnvironmentStatus"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public EnvironmentStatus(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    public static EnvironmentStatus Unspecified { get; } = new("STATUS_UNSPECIFIED");

    public static EnvironmentStatus Active { get; } = new("ACTIVE");

    public static EnvironmentStatus Expired { get; } = new("EXPIRED");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<EnvironmentStatus> KnownValues { get; } =
    [
        Unspecified,
        Active,
        Expired,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "STATUS_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(EnvironmentStatus left, EnvironmentStatus right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(EnvironmentStatus left, EnvironmentStatus right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(EnvironmentStatus other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is EnvironmentStatus other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<EnvironmentStatus>
    {
        /// <inheritdoc/>
        public override EnvironmentStatus Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for EnvironmentStatus."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, EnvironmentStatus value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

