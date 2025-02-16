using GeminiDotnet.ContentGeneration;
using GeminiDotnet.Embeddings;
using System.Text.Json.Serialization;

namespace GeminiDotnet;

[JsonSourceGenerationOptions(
    WriteIndented = false,
    RespectNullableAnnotations = true,
    UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow)]
[JsonSerializable(typeof(GenerateContentRequest))]
[JsonSerializable(typeof(GenerateContentResponse))]
[JsonSerializable(typeof(EmbeddingRequest))]
[JsonSerializable(typeof(EmbeddingResponse))]
[JsonSerializable(typeof(ErrorResponse))]
internal sealed partial class JsonContext : JsonSerializerContext;
