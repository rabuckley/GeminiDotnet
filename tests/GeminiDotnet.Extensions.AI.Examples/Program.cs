using GeminiDotnet;
using GeminiDotnet.Extensions.AI;
using GeminiDotnet.Extensions.AI.Examples;

var key = Environment.GetEnvironmentVariable("GEMINI_DOTNET_API_KEY")
    ?? throw new InvalidOperationException("Missing GEMINI_DOTNET_API_KEY environment variable");

var options = new GeminiClientOptions
{
    ApiKey = key, ModelId = GeminiModels.Gemini2Flash, ApiVersion = GeminiApiVersions.V1Beta
};

var client = new GeminiChatClient(options);

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) => cts.Cancel();

await LoggingExample.ExecuteAsync(client, cts.Token);
