using GeminiDotnet.Extensions.AI.Contents;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.Extensions.AI;

[JsonSourceGenerationOptions(
    JsonSerializerDefaults.Web,
    UseStringEnumConverter = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = true)]
[JsonSerializable(typeof(ExecutableCodeContent))]
[JsonSerializable(typeof(CodeExecutionContent))]
[JsonSerializable(typeof(IDictionary<string, object?>))]
[JsonSerializable(typeof(JsonElement))]
internal sealed partial class JsonContext : JsonSerializerContext;
