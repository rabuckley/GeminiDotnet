namespace GeminiDotnet;

public sealed record GeminiClientOptions
{
    public required string ApiKey { get; init; }
}