using System.Text.Json.Serialization;

using GeminiDotnet.ContentGeneration;
using GeminiDotnet.ContentGeneration.FunctionCalling;
using GeminiDotnet.Embeddings;

namespace GeminiDotnet;

[JsonSourceGenerationOptions(
    WriteIndented = false,
    RespectNullableAnnotations = true,
    UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow)]
[JsonSerializable(typeof(GenerateContentRequest))]
[JsonSerializable(typeof(GenerateContentResponse))]
[JsonSerializable(typeof(StreamingTextGenerationResponse))]
[JsonSerializable(typeof(EmbeddingRequest))]
[JsonSerializable(typeof(EmbeddingResponse))]
[JsonSerializable(typeof(InlineDataContentPart))]
[JsonSerializable(typeof(FileDataContentPart))]
[JsonSerializable(typeof(FunctionCallContentPart))]
[JsonSerializable(typeof(FunctionResponseContentPart))]
[JsonSerializable(typeof(ExecutableCodeContentPart))]
[JsonSerializable(typeof(CodeExecutionResultContentPart))]
internal sealed partial class JsonContext : JsonSerializerContext;