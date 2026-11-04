using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Optional. Disabled safety policies for computer use.
/// </summary>
/// <remarks>
/// Unknown API values are preserved.
/// </remarks>
[JsonConverter(typeof(ComputerUseDisabledSafetyPolicies.Converter))]
public readonly struct ComputerUseDisabledSafetyPolicies : IEquatable<ComputerUseDisabledSafetyPolicies>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComputerUseDisabledSafetyPolicies"/> structure.
    /// </summary>
    /// <param name="value">
    /// The wire value, including values not declared by this type.
    /// </param>
    /// <exception cref="global::System.ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public ComputerUseDisabledSafetyPolicies(string value)
    {
        global::System.ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    public static ComputerUseDisabledSafetyPolicies SafetyPolicyUnspecified { get; } = new("SAFETY_POLICY_UNSPECIFIED");

    public static ComputerUseDisabledSafetyPolicies FinancialTransactions { get; } = new("FINANCIAL_TRANSACTIONS");

    public static ComputerUseDisabledSafetyPolicies SensitiveDataModification { get; } = new("SENSITIVE_DATA_MODIFICATION");

    public static ComputerUseDisabledSafetyPolicies CommunicationTool { get; } = new("COMMUNICATION_TOOL");

    public static ComputerUseDisabledSafetyPolicies AccountCreation { get; } = new("ACCOUNT_CREATION");

    public static ComputerUseDisabledSafetyPolicies DataModification { get; } = new("DATA_MODIFICATION");

    public static ComputerUseDisabledSafetyPolicies UserConsentManagement { get; } = new("USER_CONSENT_MANAGEMENT");

    public static ComputerUseDisabledSafetyPolicies LegalTermsAndAgreements { get; } = new("LEGAL_TERMS_AND_AGREEMENTS");

    /// <summary>
    /// Gets the declared values in schema order.
    /// </summary>
    public static IReadOnlyList<ComputerUseDisabledSafetyPolicies> KnownValues { get; } =
    [
        SafetyPolicyUnspecified,
        FinancialTransactions,
        SensitiveDataModification,
        CommunicationTool,
        AccountCreation,
        DataModification,
        UserConsentManagement,
        LegalTermsAndAgreements,
    ];

    /// <summary>
    /// Gets the wire value.
    /// </summary>
    /// <value>
    /// The default instance has the value of <see cref="SafetyPolicyUnspecified"/>.
    /// </value>
    public string Value => _value ?? "SAFETY_POLICY_UNSPECIFIED";

    /// <summary>
    /// Compares wire values using ordinal equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator ==(ComputerUseDisabledSafetyPolicies left, ComputerUseDisabledSafetyPolicies right) => left.Equals(right);

    /// <summary>
    /// Compares wire values using ordinal inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    public static bool operator !=(ComputerUseDisabledSafetyPolicies left, ComputerUseDisabledSafetyPolicies right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(ComputerUseDisabledSafetyPolicies other) => string.Equals(Value, other.Value, global::System.StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is ComputerUseDisabledSafetyPolicies other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => global::System.StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value;

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    public sealed class Converter : JsonConverter<ComputerUseDisabledSafetyPolicies>
    {
        /// <inheritdoc/>
        public override ComputerUseDisabledSafetyPolicies Read(ref Utf8JsonReader reader, global::System.Type typeToConvert, JsonSerializerOptions options)
            => new(reader.GetString() ?? throw new JsonException("Expected a string for ComputerUseDisabledSafetyPolicies."));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ComputerUseDisabledSafetyPolicies value, JsonSerializerOptions options)
        {
            global::System.ArgumentNullException.ThrowIfNull(writer);
            writer.WriteStringValue(value.Value);
        }
    }
}

