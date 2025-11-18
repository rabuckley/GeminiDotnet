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
