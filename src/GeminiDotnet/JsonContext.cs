using GeminiDotnet.ContentGeneration;
using GeminiDotnet.Embeddings;
using System.Text.Json.Serialization;

namespace GeminiDotnet;

[JsonSourceGenerationOptions(
    WriteIndented = false,
    RespectNullableAnnotations = true
// UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
)]
[JsonSerializable(typeof(GenerateContentRequest))]
[JsonSerializable(typeof(GenerateContentResponse))]
[JsonSerializable(typeof(EmbedContentRequest))]
[JsonSerializable(typeof(EmbedContentResponse))]
[JsonSerializable(typeof(ErrorResponse))]
internal sealed partial class JsonContext : JsonSerializerContext;
