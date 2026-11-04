using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.GeneratedFiles;

/// <summary>
/// Output only. The state of the GeneratedFile.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(GeneratedFileState.Converter))]
public readonly struct GeneratedFileState : IEquatable<GeneratedFileState>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="GeneratedFileState"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public GeneratedFileState(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// The default value. This value is used if the state is omitted.
    /// </summary>
    public static GeneratedFileState Unspecified { get; } = new("STATE_UNSPECIFIED");

    /// <summary>
    /// Being generated.
    /// </summary>
    public static GeneratedFileState Generating { get; } = new("GENERATING");

    /// <summary>
    /// Generated and is ready for download.
    /// </summary>
    public static GeneratedFileState Generated { get; } = new("GENERATED");

    /// <summary>
    /// Failed to generate the GeneratedFile.
    /// </summary>
    public static GeneratedFileState Failed { get; } = new("FAILED");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<GeneratedFileState> KnownValues { get; } =
    [
        Unspecified,
        Generating,
        Generated,
        Failed,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "STATE_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(GeneratedFileState left, GeneratedFileState right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(GeneratedFileState left, GeneratedFileState right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(GeneratedFileState other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is GeneratedFileState other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<GeneratedFileState>
    {
        /// <inheritdoc/>
        public override GeneratedFileState Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for GeneratedFileState."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, GeneratedFileState value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

