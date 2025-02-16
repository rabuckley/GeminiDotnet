using Xunit.v3;

namespace GeminiDotnet.Testing;

/// <summary>
/// Used to mark tests which interact with the Gemini API.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class IntegrationTestAttribute : Attribute, ITraitAttribute
{
    public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits()
    {
        return [new KeyValuePair<string, string>("Category", "Integration")];
    }
}
