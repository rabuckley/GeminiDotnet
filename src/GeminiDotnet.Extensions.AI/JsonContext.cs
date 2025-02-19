using GeminiDotnet.ContentGeneration.FunctionCalling;
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
internal sealed partial class JsonContext : JsonSerializerContext;
