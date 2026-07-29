using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. Disabled safety policies for computer use.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ComputerUseDisabledSafetyPolicies>))]
public enum ComputerUseDisabledSafetyPolicies
{
    [JsonStringEnumMemberName("SAFETY_POLICY_UNSPECIFIED")]
    SafetyPolicyUnspecified,

    [JsonStringEnumMemberName("FINANCIAL_TRANSACTIONS")]
    FinancialTransactions,

    [JsonStringEnumMemberName("SENSITIVE_DATA_MODIFICATION")]
    SensitiveDataModification,

    [JsonStringEnumMemberName("COMMUNICATION_TOOL")]
    CommunicationTool,

    [JsonStringEnumMemberName("ACCOUNT_CREATION")]
    AccountCreation,

    [JsonStringEnumMemberName("DATA_MODIFICATION")]
    DataModification,

    [JsonStringEnumMemberName("USER_CONSENT_MANAGEMENT")]
    UserConsentManagement,

    [JsonStringEnumMemberName("LEGAL_TERMS_AND_AGREEMENTS")]
    LegalTermsAndAgreements,
}

