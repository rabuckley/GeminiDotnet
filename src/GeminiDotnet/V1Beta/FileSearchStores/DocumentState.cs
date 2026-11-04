using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// Output only. Current state of the <c>Document</c>.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(DocumentState.Converter))]
public readonly struct DocumentState : IEquatable<DocumentState>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentState"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public DocumentState(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// The default value. This value is used if the state is omitted.
    /// </summary>
    public static DocumentState Unspecified { get; } = new("STATE_UNSPECIFIED");

    /// <summary>
    /// Some <c>Chunks</c> of the <c>Document</c> are being processed (embedding and vector
    /// storage).
    /// </summary>
    public static DocumentState Pending { get; } = new("STATE_PENDING");

    /// <summary>
    /// All <c>Chunks</c> of the <c>Document</c> is processed and available for querying.
    /// </summary>
    public static DocumentState Active { get; } = new("STATE_ACTIVE");

    /// <summary>
    /// Some <c>Chunks</c> of the <c>Document</c> failed processing.
    /// </summary>
    public static DocumentState Failed { get; } = new("STATE_FAILED");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<DocumentState> KnownValues { get; } =
    [
        Unspecified,
        Pending,
        Active,
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
    public static bool operator ==(DocumentState left, DocumentState right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(DocumentState left, DocumentState right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(DocumentState other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is DocumentState other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<DocumentState>
    {
        /// <inheritdoc/>
        public override DocumentState Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for DocumentState."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, DocumentState value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

