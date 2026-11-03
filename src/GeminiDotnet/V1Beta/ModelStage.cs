using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(ModelStage.Converter))]
public readonly struct ModelStage : IEquatable<ModelStage>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModelStage"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public ModelStage(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Unspecified model stage.
    /// </summary>
    public static ModelStage Unspecified { get; } = new("MODEL_STAGE_UNSPECIFIED");

    /// <summary>
    /// The underlying model is subject to lots of tunings.
    /// </summary>
    public static ModelStage UnstableExperimental { get; } = new("UNSTABLE_EXPERIMENTAL");

    /// <summary>
    /// Models in this stage are for experimental purposes only.
    /// </summary>
    public static ModelStage Experimental { get; } = new("EXPERIMENTAL");

    /// <summary>
    /// Models in this stage are more mature than experimental models.
    /// </summary>
    public static ModelStage Preview { get; } = new("PREVIEW");

    /// <summary>
    /// Models in this stage are considered stable and ready for production use.
    /// </summary>
    public static ModelStage Stable { get; } = new("STABLE");

    /// <summary>
    /// If the model is on this stage, it means that this model is on the path to
    /// deprecation in near future. Only existing customers can use this model.
    /// </summary>
    public static ModelStage Legacy { get; } = new("LEGACY");

    /// <summary>
    /// Models in this stage are deprecated. These models cannot be used.
    /// </summary>
    public static ModelStage Deprecated { get; } = new("DEPRECATED");

    /// <summary>
    /// Models in this stage are retired. These models cannot be used.
    /// </summary>
    public static ModelStage Retired { get; } = new("RETIRED");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<ModelStage> KnownValues { get; } =
    [
        Unspecified,
        UnstableExperimental,
        Experimental,
        Preview,
        Stable,
        Legacy,
        Deprecated,
        Retired,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "MODEL_STAGE_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(ModelStage left, ModelStage right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(ModelStage left, ModelStage right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(ModelStage other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is ModelStage other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<ModelStage>
    {
        /// <inheritdoc/>
        public override ModelStage Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for ModelStage."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ModelStage value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

