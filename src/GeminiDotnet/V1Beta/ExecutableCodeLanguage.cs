using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Required. Programming language of the <c>code</c>.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(ExecutableCodeLanguage.Converter))]
public readonly struct ExecutableCodeLanguage : IEquatable<ExecutableCodeLanguage>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutableCodeLanguage"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public ExecutableCodeLanguage(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Unspecified language. This value should not be used.
    /// </summary>
    public static ExecutableCodeLanguage Unspecified { get; } = new("LANGUAGE_UNSPECIFIED");

    /// <summary>
    /// Python >= 3.10, with numpy and simpy available.
    /// Python is the default language.
    /// </summary>
    public static ExecutableCodeLanguage Python { get; } = new("PYTHON");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<ExecutableCodeLanguage> KnownValues { get; } =
    [
        Unspecified,
        Python,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "LANGUAGE_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(ExecutableCodeLanguage left, ExecutableCodeLanguage right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(ExecutableCodeLanguage left, ExecutableCodeLanguage right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(ExecutableCodeLanguage other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is ExecutableCodeLanguage other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<ExecutableCodeLanguage>
    {
        /// <inheritdoc/>
        public override ExecutableCodeLanguage Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for ExecutableCodeLanguage."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ExecutableCodeLanguage value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

