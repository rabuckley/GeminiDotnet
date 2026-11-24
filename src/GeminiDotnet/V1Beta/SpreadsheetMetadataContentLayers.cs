using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. Content layers to return.
/// If empty, defaults to [DATA, FORMULAS] when SpreadsheetMetadata is set.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
[JsonConverter(typeof(SpreadsheetMetadataContentLayers.Converter))]
public readonly struct SpreadsheetMetadataContentLayers : IEquatable<SpreadsheetMetadataContentLayers>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpreadsheetMetadataContentLayers"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public SpreadsheetMetadataContentLayers(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    public static SpreadsheetMetadataContentLayers ContentLayerUnspecified { get; } = new("CONTENT_LAYER_UNSPECIFIED");

    public static SpreadsheetMetadataContentLayers Data { get; } = new("DATA");

    public static SpreadsheetMetadataContentLayers Formulas { get; } = new("FORMULAS");

    public static SpreadsheetMetadataContentLayers Visual { get; } = new("VISUAL");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<SpreadsheetMetadataContentLayers> KnownValues { get; } =
    [
        ContentLayerUnspecified,
        Data,
        Formulas,
        Visual,
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
    public static bool operator ==(SpreadsheetMetadataContentLayers left, SpreadsheetMetadataContentLayers right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(SpreadsheetMetadataContentLayers left, SpreadsheetMetadataContentLayers right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(SpreadsheetMetadataContentLayers other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is SpreadsheetMetadataContentLayers other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<SpreadsheetMetadataContentLayers>
    {
        /// <inheritdoc/>
        public override SpreadsheetMetadataContentLayers Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for SpreadsheetMetadataContentLayers."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, SpreadsheetMetadataContentLayers value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

