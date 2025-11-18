using System.Text.Json.Serialization;

namespace GeminiDotnet;

[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(Dictionary<string, object>))]
[JsonSerializable(typeof(ErrorResponse))]
[JsonSerializable(typeof(GeminiDotnet.V1.ListOperationsResponse))]
[JsonSerializable(typeof(GeminiDotnet.V1.Operation))]
[JsonSerializable(typeof(GeminiDotnet.V1.Empty))]
[JsonSerializable(typeof(GeminiDotnet.V1.Batches.EmbedContentBatch))]
[JsonSerializable(typeof(GeminiDotnet.V1.Batches.GenerateContentBatch))]
[JsonSerializable(typeof(GeminiDotnet.V1.GenerateContentRequest))]
[JsonSerializable(typeof(GeminiDotnet.V1.GenerateContentResponse))]
[JsonSerializable(typeof(GeminiDotnet.V1.Models.ListModelsResponse))]
[JsonSerializable(typeof(GeminiDotnet.V1.Models.Model))]
[JsonSerializable(typeof(GeminiDotnet.V1.AsyncBatchEmbedContentRequest))]
[JsonSerializable(typeof(GeminiDotnet.V1.AsyncBatchEmbedContentOperation))]
[JsonSerializable(typeof(GeminiDotnet.V1.Models.BatchEmbedContentsRequest))]
[JsonSerializable(typeof(GeminiDotnet.V1.Models.BatchEmbedContentsResponse))]
[JsonSerializable(typeof(GeminiDotnet.V1.BatchGenerateContentRequest))]
[JsonSerializable(typeof(GeminiDotnet.V1.BatchGenerateContentOperation))]
[JsonSerializable(typeof(GeminiDotnet.V1.Models.CountTokensRequest))]
[JsonSerializable(typeof(GeminiDotnet.V1.Models.CountTokensResponse))]
[JsonSerializable(typeof(GeminiDotnet.V1.Models.EmbedContentRequest))]
[JsonSerializable(typeof(GeminiDotnet.V1.Models.EmbedContentResponse))]
[JsonSerializable(typeof(GeminiDotnet.V1.TunedModels.CancelOperationRequest))]
internal partial class V1JsonContext : JsonSerializerContext;
