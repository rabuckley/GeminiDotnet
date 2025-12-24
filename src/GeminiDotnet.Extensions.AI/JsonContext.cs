using GeminiDotnet.Extensions.AI.Contents;
using GeminiDotnet.V1Beta;
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
// Types used in ChatOptions.AdditionalProperties (for SK PromptExecutionSettings compatibility)
[JsonSerializable(typeof(ThinkingConfiguration))]
[JsonSerializable(typeof(ImageConfiguration))]
[JsonSerializable(typeof(ResponseModality))]
[JsonSerializable(typeof(IEnumerable<ResponseModality>))]
[JsonSerializable(typeof(List<ResponseModality>))]
internal sealed partial class JsonContext : JsonSerializerContext;
