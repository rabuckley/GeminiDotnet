using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeminiDotnet.V1Beta.AuthTokens;
using GeminiDotnet.V1Beta.CachedContents;
using GeminiDotnet.V1Beta.Corpora;
using GeminiDotnet.V1Beta.Files;
using GeminiDotnet.V1Beta.FileSearchStores;
using GeminiDotnet.V1Beta.FilesRegister;
using GeminiDotnet.V1Beta.GeneratedFiles;
using GeminiDotnet.V1Beta.Models;
using GeminiDotnet.V1Beta.TunedModels;
using File = GeminiDotnet.V1Beta.Files.File;

namespace GeminiDotnet.V1Beta;

internal sealed class DynamicClient : IDynamicClient
{
    private readonly IGeminiRequester _requester;
    
    internal DynamicClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<GenerateContentResponse> GenerateContentByDynamicIdAsync(
        string dynamicId,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dynamicId);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/dynamic/{dynamicId}:generateContent";
        return _requester.ExecuteAsync<GenerateContentRequest, GenerateContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

    public IAsyncEnumerable<GenerateContentResponse> StreamGenerateContentByDynamicIdAsync(
        string dynamicId,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dynamicId);
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/dynamic/{dynamicId}:streamGenerateContent?alt=sse";
        return _requester.ExecuteStreamingAsync<GenerateContentRequest, GenerateContentResponse>(HttpMethod.Post, path, request, cancellationToken);
    }

}
