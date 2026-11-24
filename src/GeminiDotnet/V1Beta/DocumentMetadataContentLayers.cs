using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. Content layers to return.
/// If empty, defaults to [PAGE_IMAGE] when DocumentMetadata is set. When
/// DocumentMetadata is not set, default document processing is used (for
/// example, page images for PDFs).
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
[JsonConverter(typeof(DocumentMetadataContentLayers.Converter))]
public readonly struct DocumentMetadataContentLayers : IEquatable<DocumentMetadataContentLayers>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentMetadataContentLayers"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public DocumentMetadataContentLayers(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    public static DocumentMetadataContentLayers ContentLayerUnspecified { get; } = new("CONTENT_LAYER_UNSPECIFIED");

    public static DocumentMetadataContentLayers PageImage { get; } = new("PAGE_IMAGE");

    public static DocumentMetadataContentLayers PageText { get; } = new("PAGE_TEXT");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<DocumentMetadataContentLayers> KnownValues { get; } =
    [
        ContentLayerUnspecified,
        PageImage,
        PageText,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="ContentLayerUnspecified"/>.
    /// </value>
    public string Value => _value ?? "CONTENT_LAYER_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(DocumentMetadataContentLayers left, DocumentMetadataContentLayers right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(DocumentMetadataContentLayers left, DocumentMetadataContentLayers right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(DocumentMetadataContentLayers other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is DocumentMetadataContentLayers other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<DocumentMetadataContentLayers>
    {
        /// <inheritdoc/>
        public override DocumentMetadataContentLayers Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for DocumentMetadataContentLayers."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, DocumentMetadataContentLayers value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

