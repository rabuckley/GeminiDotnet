using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeminiDotnet.V1Beta.FileSearchStores;

namespace GeminiDotnet.V1Beta;

internal sealed partial class FileSearchStoresDocumentsClient : IFileSearchStoresDocumentsClient
{
    private readonly IGeminiRequester _requester;
    
    internal FileSearchStoresDocumentsClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<ListDocumentsResponse> ListAsync(
        string parent,
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(parent);
        if (parent.Split('/') is not ["fileSearchStores", { Length: > 0 }])
        {
            throw new ArgumentException($"'{parent}' is not a resource name of the form 'fileSearchStores/{{fileSearchStoreId}}'.", nameof(parent));
        }
        var query = new QueryStringBuilder()
            .Add("pageSize", pageSize)
            .Add("pageToken", pageToken)
            .ToString();
        var path = $"/v1beta/{WildcardPath.Escape(parent)}/documents{query}";
        return _requester.ExecuteAsync<ListDocumentsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Document> GetAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["fileSearchStores", { Length: > 0 }, "documents", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'fileSearchStores/{{fileSearchStoreId}}/documents/{{documentId}}'.", nameof(name));
        }
        var path = $"/v1beta/{WildcardPath.Escape(name)}";
        return _requester.ExecuteAsync<Document>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeleteAsync(
        string name,
        bool? force = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["fileSearchStores", { Length: > 0 }, "documents", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'fileSearchStores/{{fileSearchStoreId}}/documents/{{documentId}}'.", nameof(name));
        }
        var query = new QueryStringBuilder()
            .Add("force", force)
            .ToString();
        var path = $"/v1beta/{WildcardPath.Escape(name)}{query}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

}
