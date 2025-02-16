namespace GeminiDotnet.Extensions.AI;

public static class TestConfiguration
{
    private const string VariableName = "GEMINI_DOTNET_API_KEY";

    public static string GetApiKey()
    {
        return Environment.GetEnvironmentVariable(VariableName)
            ?? throw new InvalidOperationException($"Environment variable '{VariableName}' is not set.");
    }
}
