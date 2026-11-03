using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Required. The environment being operated.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(ComputerUseEnvironment.Converter))]
public readonly struct ComputerUseEnvironment : IEquatable<ComputerUseEnvironment>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComputerUseEnvironment"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public ComputerUseEnvironment(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Defaults to browser.
    /// </summary>
    public static ComputerUseEnvironment Unspecified { get; } = new("ENVIRONMENT_UNSPECIFIED");

    /// <summary>
    /// Operates in a web browser.
    /// </summary>
    public static ComputerUseEnvironment Browser { get; } = new("ENVIRONMENT_BROWSER");

    /// <summary>
    /// Operates in a mobile environment.
    /// </summary>
    public static ComputerUseEnvironment Mobile { get; } = new("ENVIRONMENT_MOBILE");

    /// <summary>
    /// Operates in a desktop environment.
    /// </summary>
    public static ComputerUseEnvironment Desktop { get; } = new("ENVIRONMENT_DESKTOP");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<ComputerUseEnvironment> KnownValues { get; } =
    [
        Unspecified,
        Browser,
        Mobile,
        Desktop,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="Unspecified"/>.
    /// </value>
    public string Value => _value ?? "ENVIRONMENT_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(ComputerUseEnvironment left, ComputerUseEnvironment right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(ComputerUseEnvironment left, ComputerUseEnvironment right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(ComputerUseEnvironment other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is ComputerUseEnvironment other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<ComputerUseEnvironment>
    {
        /// <inheritdoc/>
        public override ComputerUseEnvironment Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for ComputerUseEnvironment."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ComputerUseEnvironment value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

