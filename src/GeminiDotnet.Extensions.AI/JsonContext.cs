using GeminiDotnet.Extensions.AI.Contents;
using GeminiDotnet.V1Beta;
using System.Text.Json;
using System.Text.Json.Serialization;

#pragma warning disable CS0618 // Obsolete types kept for backward-compatible JSON serialization

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
// Fallback for the JsonElement path in AdditionalPropertiesDictionaryExtensions.TryConvertValue<T>.
[JsonSerializable(typeof(string))]
// Types used in ChatOptions.AdditionalProperties (for SK PromptExecutionSettings compatibility)
[JsonSerializable(typeof(ThinkingConfiguration))]
[JsonSerializable(typeof(ImageConfiguration))]
[JsonSerializable(typeof(ResponseModality))]
[JsonSerializable(typeof(IEnumerable<ResponseModality>))]
[JsonSerializable(typeof(List<ResponseModality>))]
// Stored in AIContent.AdditionalProperties under GeminiContentProperties.ToolType and .Outcome. These
// registrations let GeminiJsonUtilities.DefaultOptions both write the enums under AOT and read them back
// through AdditionalPropertiesDictionaryExtensions.TryConvertValue<T>.
[JsonSerializable(typeof(ToolType))]
[JsonSerializable(typeof(CodeExecutionResultOutcome))]
internal sealed partial class JsonContext : JsonSerializerContext;
