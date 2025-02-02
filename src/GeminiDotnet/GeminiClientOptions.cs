namespace GeminiDotnet;

public sealed record GeminiClientOptions
{
    public required string ApiKey { get; init; }

    public string ApiVersion { get; set; } = GeminiApiVersions.V1;
}