using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeminiDotnet.V1Beta.AuthTokens;
using GeminiDotnet.V1Beta.CachedContents;
using GeminiDotnet.V1Beta.Corpora;
using GeminiDotnet.V1Beta.Environments;
using GeminiDotnet.V1Beta.EnvironmentsCreate;
using GeminiDotnet.V1Beta.EnvironmentsList;
using GeminiDotnet.V1Beta.Files;
using GeminiDotnet.V1Beta.FileSearchStores;
using GeminiDotnet.V1Beta.FilesRegister;
using GeminiDotnet.V1Beta.GeneratedFiles;
using GeminiDotnet.V1Beta.Models;
using GeminiDotnet.V1Beta.TunedModels;
using File = GeminiDotnet.V1Beta.Files.File;

namespace GeminiDotnet.V1Beta;

internal sealed class BatchesClient : IBatchesClient
{
    private readonly IGeminiRequester _requester;
    
    internal BatchesClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<ListOperationsResponse> ListOperationsByAsync(
        string? filter = null,
        int? pageSize = null,
        string? pageToken = null,
        bool? returnPartialSuccess = null,
        CancellationToken cancellationToken = default)
    {
        var query = new QueryStringBuilder()
            .Add("filter", filter)
            .Add("pageSize", pageSize)
            .Add("pageToken", pageToken)
            .Add("returnPartialSuccess", returnPartialSuccess)
            .ToString();
        var path = $"/v1beta/batches{query}";
        return _requester.ExecuteAsync<ListOperationsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Operation> GetOperationByGenerateContentBatchAsync(
        string generateContentBatch,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(generateContentBatch);
        var path = $"/v1beta/batches/{Uri.EscapeDataString(generateContentBatch)}";
        return _requester.ExecuteAsync<Operation>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeleteOperationAsync(
        string generateContentBatch,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(generateContentBatch);
        var path = $"/v1beta/batches/{Uri.EscapeDataString(generateContentBatch)}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<Empty> CancelOperationAsync(
        string generateContentBatch,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(generateContentBatch);
        var path = $"/v1beta/batches/{Uri.EscapeDataString(generateContentBatch)}:cancel";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Post, path, cancellationToken);
    }

    public Task<EmbedContentBatch> UpdateEmbedContentBatchAsync(
        string generateContentBatch,
        EmbedContentBatch request,
        string? updateMask = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(generateContentBatch);
        ArgumentNullException.ThrowIfNull(request);
        var query = new QueryStringBuilder()
            .Add("updateMask", updateMask)
            .ToString();
        var path = $"/v1beta/batches/{Uri.EscapeDataString(generateContentBatch)}:updateEmbedContentBatch{query}";
        return _requester.ExecuteAsync<EmbedContentBatch, EmbedContentBatch>(HttpMethod.Patch, path, request, cancellationToken);
    }

    public Task<GenerateContentBatch> UpdateGenerateContentBatchAsync(
        string generateContentBatch,
        GenerateContentBatch request,
        string? updateMask = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(generateContentBatch);
        ArgumentNullException.ThrowIfNull(request);
        var query = new QueryStringBuilder()
            .Add("updateMask", updateMask)
            .ToString();
        var path = $"/v1beta/batches/{Uri.EscapeDataString(generateContentBatch)}:updateGenerateContentBatch{query}";
        return _requester.ExecuteAsync<GenerateContentBatch, GenerateContentBatch>(HttpMethod.Patch, path, request, cancellationToken);
    }

}
