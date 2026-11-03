using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Environments;

/// <summary>
/// Output only. The type of the entry.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(EnvironmentFileType.Converter))]
public readonly struct EnvironmentFileType : IEquatable<EnvironmentFileType>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnvironmentFileType"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public EnvironmentFileType(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Unspecified type.
    /// </summary>
    public static EnvironmentFileType Unspecified { get; } = new("TYPE_UNSPECIFIED");

    /// <summary>
    /// A regular file.
    /// </summary>
    public static EnvironmentFileType File { get; } = new("FILE");

    /// <summary>
    /// A directory.
    /// </summary>
    public static EnvironmentFileType Directory { get; } = new("DIRECTORY");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<EnvironmentFileType> KnownValues { get; } =
    [
        Unspecified,
        File,
        Directory,
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
    public static bool operator ==(EnvironmentFileType left, EnvironmentFileType right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(EnvironmentFileType left, EnvironmentFileType right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(EnvironmentFileType other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is EnvironmentFileType other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<EnvironmentFileType>
    {
        /// <inheritdoc/>
        public override EnvironmentFileType Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for EnvironmentFileType."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, EnvironmentFileType value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

