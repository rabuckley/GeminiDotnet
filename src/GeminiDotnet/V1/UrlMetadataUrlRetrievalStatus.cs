using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Status of the url retrieval.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(UrlMetadataUrlRetrievalStatus.Converter))]
public readonly struct UrlMetadataUrlRetrievalStatus : IEquatable<UrlMetadataUrlRetrievalStatus>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="UrlMetadataUrlRetrievalStatus"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public UrlMetadataUrlRetrievalStatus(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Default value. This value is unused.
    /// </summary>
    public static UrlMetadataUrlRetrievalStatus Unspecified { get; } = new("URL_RETRIEVAL_STATUS_UNSPECIFIED");

    /// <summary>
    /// Url retrieval is successful.
    /// </summary>
    public static UrlMetadataUrlRetrievalStatus Success { get; } = new("URL_RETRIEVAL_STATUS_SUCCESS");

    /// <summary>
    /// Url retrieval is failed due to error.
    /// </summary>
    public static UrlMetadataUrlRetrievalStatus Error { get; } = new("URL_RETRIEVAL_STATUS_ERROR");

    /// <summary>
    /// Url retrieval is failed because the content is behind paywall.
    /// </summary>
    public static UrlMetadataUrlRetrievalStatus Paywall { get; } = new("URL_RETRIEVAL_STATUS_PAYWALL");

    /// <summary>
    /// Url retrieval is failed because the content is unsafe.
    /// </summary>
    public static UrlMetadataUrlRetrievalStatus Unsafe { get; } = new("URL_RETRIEVAL_STATUS_UNSAFE");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<UrlMetadataUrlRetrievalStatus> KnownValues { get; } =
    [
        Unspecified,
        Success,
        Error,
        Paywall,
        Unsafe,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "URL_RETRIEVAL_STATUS_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(UrlMetadataUrlRetrievalStatus left, UrlMetadataUrlRetrievalStatus right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(UrlMetadataUrlRetrievalStatus left, UrlMetadataUrlRetrievalStatus right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(UrlMetadataUrlRetrievalStatus other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is UrlMetadataUrlRetrievalStatus other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<UrlMetadataUrlRetrievalStatus>
    {
        /// <inheritdoc/>
        public override UrlMetadataUrlRetrievalStatus Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for UrlMetadataUrlRetrievalStatus."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, UrlMetadataUrlRetrievalStatus value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

