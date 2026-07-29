namespace GeminiDotnet.Testing;

public static class TestConfiguration
{
    private const string VariableName = "GEMINI_DOTNET_API_KEY";

    /// <summary>
    /// The default Gemini model that the integration test suite runs against.
    /// </summary>
    public const string DefaultModel = "gemini-3.1-flash-lite";

    public static string GetApiKey()
    {
        return Environment.GetEnvironmentVariable(VariableName)
            ?? throw new InvalidOperationException($"Environment variable '{VariableName}' is not set.");
    }
}
