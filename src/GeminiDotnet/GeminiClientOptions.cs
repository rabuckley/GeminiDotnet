namespace GeminiDotnet;

public sealed record GeminiClientOptions
{
    public required string ApiKey { get; set; }

    public string ApiVersion { get; set; } = GeminiApiVersions.V1;
}