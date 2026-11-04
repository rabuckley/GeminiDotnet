using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Required. Style in which answers should be returned.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(GenerateAnswerRequestAnswerStyle.Converter))]
public readonly struct GenerateAnswerRequestAnswerStyle : IEquatable<GenerateAnswerRequestAnswerStyle>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenerateAnswerRequestAnswerStyle"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public GenerateAnswerRequestAnswerStyle(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Unspecified answer style.
    /// </summary>
    public static GenerateAnswerRequestAnswerStyle Unspecified { get; } = new("ANSWER_STYLE_UNSPECIFIED");

    /// <summary>
    /// Succinct but abstract style.
    /// </summary>
    public static GenerateAnswerRequestAnswerStyle Abstractive { get; } = new("ABSTRACTIVE");

    /// <summary>
    /// Very brief and extractive style.
    /// </summary>
    public static GenerateAnswerRequestAnswerStyle Extractive { get; } = new("EXTRACTIVE");

    /// <summary>
    /// Verbose style including extra details. The response may be formatted as a
    /// sentence, paragraph, multiple paragraphs, or bullet points, etc.
    /// </summary>
    public static GenerateAnswerRequestAnswerStyle Verbose { get; } = new("VERBOSE");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<GenerateAnswerRequestAnswerStyle> KnownValues { get; } =
    [
        Unspecified,
        Abstractive,
        Extractive,
        Verbose,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "ANSWER_STYLE_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(GenerateAnswerRequestAnswerStyle left, GenerateAnswerRequestAnswerStyle right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(GenerateAnswerRequestAnswerStyle left, GenerateAnswerRequestAnswerStyle right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(GenerateAnswerRequestAnswerStyle other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is GenerateAnswerRequestAnswerStyle other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<GenerateAnswerRequestAnswerStyle>
    {
        /// <inheritdoc/>
        public override GenerateAnswerRequestAnswerStyle Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for GenerateAnswerRequestAnswerStyle."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, GenerateAnswerRequestAnswerStyle value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

