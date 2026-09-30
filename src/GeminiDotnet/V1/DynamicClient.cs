using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace GeminiDotnet.V1;

internal sealed partial class DynamicClient : IDynamicClient
{
    private readonly IGeminiRequester _requester;
    
    internal DynamicClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<GenerateContentResponse> GenerateContentAsync(
        string model,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.Split('/') is not ["dynamic", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'dynamic/{{dynamicId}}'.", nameof(model));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/{WildcardPath.Escape(model)}:generateContent";
        return _requester.ExecuteAsync<GenerateContentRequest, GenerateContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public IAsyncEnumerable<GenerateContentResponse> StreamGenerateContentAsync(
        string model,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.Split('/') is not ["dynamic", { Length: > 0 }])
        {
            throw new ArgumentException($"'{model}' is not a resource name of the form 'dynamic/{{dynamicId}}'.", nameof(model));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1/{WildcardPath.Escape(model)}:streamGenerateContent?alt=sse";
        return _requester.ExecuteStreamingAsync<GenerateContentRequest, GenerateContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

}
