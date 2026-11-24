using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeminiDotnet.V1Beta.Interactions;

namespace GeminiDotnet.V1Beta;

[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
internal sealed partial class InteractionsAuthTokensClient : IInteractionsAuthTokensClient
{
    private readonly IGeminiRequester _requester;

    internal InteractionsAuthTokensClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<HttpBody> CreateAsync(
        string name,
        HttpBody request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["interactions", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'interactions/{{interactionId}}'.", nameof(name));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/{WildcardPath.Escape(name)}/auth_tokens";
        return _requester.ExecuteAsync<HttpBody, HttpBody>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<HttpBody> DeleteAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["interactions", { Length: > 0 }, "auth_tokens", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'interactions/{{interactionId}}/auth_tokens/{{authTokenId}}'.", nameof(name));
        }
        var path = $"/v1beta/{WildcardPath.Escape(name)}";
        return _requester.ExecuteAsync<HttpBody>(HttpMethod.Delete, path, cancellationToken);
    }

}
