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

Dictionary<int, Func<GeminiChatClient, CancellationToken, Task>> examples = new()
{
    { 1, FunctionCallingExample.ExecuteAsync }, 
    { 2, LoggingExample.ExecuteAsync }
};

Console.WriteLine("Enter the number of the example you'd like to run:\n");

foreach (var ex in examples)
{
    var method = ex.Value.Method;
    Console.WriteLine($"{ex.Key}. {method.DeclaringType?.Name}");
}

if (!int.TryParse(Console.ReadLine(), out var choice) || !examples.TryGetValue(choice, out var example))
{
    Console.Error.WriteLine("Invalid choice. Please enter a valid number from the list.");
    return -1;
}

await example(client, cts.Token);
return 0;
