using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeminiDotnet.V1Beta.Corpora;

namespace GeminiDotnet.V1Beta;

internal sealed partial class CorporaClient : ICorporaClient
{
    private readonly IGeminiRequester _requester;
    
    internal CorporaClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public ICorporaOperationsClient Operations => field ??= new CorporaOperationsClient(_requester);

    public ICorporaPermissionsClient Permissions => field ??= new CorporaPermissionsClient(_requester);

    [Obsolete]
    public Task<ListCorporaResponse> ListAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new QueryStringBuilder()
            .Add("pageSize", pageSize)
            .Add("pageToken", pageToken)
            .ToString();
        var path = $"/v1beta/corpora{query}";
        return _requester.ExecuteAsync<ListCorporaResponse>(HttpMethod.Get, path, cancellationToken);
    }

    [Obsolete]
    public Task<Corpus> CreateAsync(
        Corpus request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        const string path = "/v1beta/corpora";
        return _requester.ExecuteAsync<Corpus, Corpus>(HttpMethod.Post, path, request, cancellationToken);
    }

    [Obsolete]
    public Task<Corpus> GetAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["corpora", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'corpora/{{corpusId}}'.", nameof(name));
        }
        var path = $"/v1beta/{WildcardPath.Escape(name)}";
        return _requester.ExecuteAsync<Corpus>(HttpMethod.Get, path, cancellationToken);
    }

    [Obsolete]
    public Task<Empty> DeleteAsync(
        string name,
        bool? force = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["corpora", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'corpora/{{corpusId}}'.", nameof(name));
        }
        var query = new QueryStringBuilder()
            .Add("force", force)
            .ToString();
        var path = $"/v1beta/{WildcardPath.Escape(name)}{query}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

}
