using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. Specifies the function Behavior.
/// Currently only supported by the BidiGenerateContent method.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(FunctionDeclarationBehavior.Converter))]
public readonly struct FunctionDeclarationBehavior : IEquatable<FunctionDeclarationBehavior>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionDeclarationBehavior"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public FunctionDeclarationBehavior(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// This value is unused.
    /// </summary>
    public static FunctionDeclarationBehavior Unspecified { get; } = new("UNSPECIFIED");

    /// <summary>
    /// If set, the system will wait to receive the function response before
    /// continuing the conversation.
    /// </summary>
    public static FunctionDeclarationBehavior Blocking { get; } = new("BLOCKING");

    /// <summary>
    /// If set, the system will not wait to receive the function response.
    /// Instead, it will attempt to handle function responses as they become
    /// available while maintaining the conversation between the user and the
    /// model.
    /// </summary>
    public static FunctionDeclarationBehavior NonBlocking { get; } = new("NON_BLOCKING");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<FunctionDeclarationBehavior> KnownValues { get; } =
    [
        Unspecified,
        Blocking,
        NonBlocking,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(FunctionDeclarationBehavior left, FunctionDeclarationBehavior right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(FunctionDeclarationBehavior left, FunctionDeclarationBehavior right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(FunctionDeclarationBehavior other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is FunctionDeclarationBehavior other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<FunctionDeclarationBehavior>
    {
        /// <inheritdoc/>
        public override FunctionDeclarationBehavior Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for FunctionDeclarationBehavior."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, FunctionDeclarationBehavior value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

