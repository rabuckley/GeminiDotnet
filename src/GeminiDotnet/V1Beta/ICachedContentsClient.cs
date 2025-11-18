using GeminiDotnet.V1Beta.CachedContents;

namespace GeminiDotnet.V1Beta;

public interface ICachedContentsClient
{
    /// <summary>
    /// Lists CachedContents.
    /// </summary>
    /// <param name="pageSize">
    /// Optional. The maximum number of cached contents to return. The service may return
    /// fewer than this value.
    /// If unspecified, some default (under maximum) number of items will be
    /// returned.
    /// The maximum value is 1000; values above 1000 will be coerced to 1000.
    /// </param>
    /// <param name="pageToken">
    /// Optional. A page token, received from a previous <c>ListCachedContents</c> call.
    /// Provide this to retrieve the subsequent page.
    /// When paginating, all other parameters provided to <c>ListCachedContents</c> must
    /// match the call that provided the page token.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<ListCachedContentsResponse> ListCachedContentsAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates CachedContent resource.
    /// </summary>
    /// <param name="request">
    /// Required. The cached content to create.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<CachedContent> CreateCachedContentAsync(
        CachedContent request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Reads CachedContent resource.
    /// </summary>
    /// <param name="id">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<CachedContent> GetCachedContentAsync(
        string id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes CachedContent resource.
    /// </summary>
    /// <param name="id">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Empty> DeleteCachedContentAsync(
        string id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates CachedContent resource (only expiration is updatable).
    /// </summary>
    /// <param name="id">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="request">
    /// Required. The content cache entry to update
    /// </param>
    /// <param name="updateMask">
    /// The list of fields to update.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<CachedContent> UpdateCachedContentAsync(
        string id,
        CachedContent request,
        string? updateMask = null,
        CancellationToken cancellationToken = default);

}
