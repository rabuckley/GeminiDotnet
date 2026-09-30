using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace GeminiDotnet.V1;

internal sealed partial class BatchesClient : IBatchesClient
{
    private readonly IGeminiRequester _requester;
    
    internal BatchesClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<ListOperationsResponse> ListAsync(
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
        var path = $"/v1/batches{query}";
        return _requester.ExecuteAsync<ListOperationsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Operation> GetAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["batches", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'batches/{{batchId}}'.", nameof(name));
        }
        var path = $"/v1/{WildcardPath.Escape(name)}";
        return _requester.ExecuteAsync<Operation>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeleteAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["batches", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'batches/{{batchId}}'.", nameof(name));
        }
        var path = $"/v1/{WildcardPath.Escape(name)}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<Empty> CancelAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["batches", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'batches/{{batchId}}'.", nameof(name));
        }
        var path = $"/v1/{WildcardPath.Escape(name)}:cancel";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Post, path, cancellationToken);
    }

    public Task<EmbedContentBatch> UpdateEmbedContentBatchAsync(
        string name,
        EmbedContentBatch request,
        string? updateMask = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["batches", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'batches/{{batchId}}'.", nameof(name));
        }
        ArgumentNullException.ThrowIfNull(request);
        var query = new QueryStringBuilder()
            .Add("updateMask", updateMask)
            .ToString();
        var path = $"/v1/{WildcardPath.Escape(name)}:updateEmbedContentBatch{query}";
        return _requester.ExecuteAsync<EmbedContentBatch, EmbedContentBatch>(HttpMethod.Patch, path, request, cancellationToken);
    }

    public Task<GenerateContentBatch> UpdateGenerateContentBatchAsync(
        string name,
        GenerateContentBatch request,
        string? updateMask = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["batches", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'batches/{{batchId}}'.", nameof(name));
        }
        ArgumentNullException.ThrowIfNull(request);
        var query = new QueryStringBuilder()
            .Add("updateMask", updateMask)
            .ToString();
        var path = $"/v1/{WildcardPath.Escape(name)}:updateGenerateContentBatch{query}";
        return _requester.ExecuteAsync<GenerateContentBatch, GenerateContentBatch>(HttpMethod.Patch, path, request, cancellationToken);
    }

}
