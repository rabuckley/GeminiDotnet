using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// The reason content was blocked during request processing.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(ContentFilterReason.Converter))]
public readonly struct ContentFilterReason : IEquatable<ContentFilterReason>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentFilterReason"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public ContentFilterReason(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// A blocked reason was not specified.
    /// </summary>
    public static ContentFilterReason BlockedReasonUnspecified { get; } = new("BLOCKED_REASON_UNSPECIFIED");

    /// <summary>
    /// Content was blocked by safety settings.
    /// </summary>
    public static ContentFilterReason Safety { get; } = new("SAFETY");

    /// <summary>
    /// Content was blocked, but the reason is uncategorized.
    /// </summary>
    public static ContentFilterReason Other { get; } = new("OTHER");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<ContentFilterReason> KnownValues { get; } =
    [
        BlockedReasonUnspecified,
        Safety,
        Other,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="BlockedReasonUnspecified"/>.
    /// </value>
    public string Value => _value ?? "BLOCKED_REASON_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(ContentFilterReason left, ContentFilterReason right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(ContentFilterReason left, ContentFilterReason right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(ContentFilterReason other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is ContentFilterReason other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<ContentFilterReason>
    {
        /// <inheritdoc/>
        public override ContentFilterReason Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for ContentFilterReason."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ContentFilterReason value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

