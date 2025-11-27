using GeminiDotnet.V1Beta.Corpora;

namespace GeminiDotnet.V1Beta;

public interface ICorporaClient
{
    /// <summary>
    /// Lists all <c>Corpora</c> owned by the user.
    /// </summary>
    /// <param name="pageSize">
    /// Optional. The maximum number of <c>Corpora</c> to return (per page).
    /// The service may return fewer <c>Corpora</c>.
    /// If unspecified, at most 10 <c>Corpora</c> will be returned.
    /// The maximum size limit is 20 <c>Corpora</c> per page.
    /// </param>
    /// <param name="pageToken">
    /// Optional. A page token, received from a previous <c>ListCorpora</c> call.
    /// Provide the <c>next_page_token</c> returned in the response as an argument to
    /// the next request to retrieve the next page.
    /// When paginating, all other parameters provided to <c>ListCorpora</c>
    /// must match the call that provided the page token.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<ListCorporaResponse> ListCorporaAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates an empty <see cref="V1Beta.Corpora.Corpus"/>.
    /// </summary>
    /// <param name="request">
    /// Required. The <see cref="V1Beta.Corpora.Corpus"/> to create.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Corpus> CreateCorpusAsync(
        Corpus request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets information about a specific <see cref="V1Beta.Corpora.Corpus"/>.
    /// </summary>
    /// <param name="corpus">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Corpus> GetCorpusAsync(
        string corpus,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a <see cref="V1Beta.Corpora.Corpus"/>.
    /// </summary>
    /// <param name="corpus">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="force">
    /// Optional. If set to true, any <see cref="V1Beta.FileSearchStores.Document"/>s and objects related to this <see cref="V1Beta.Corpora.Corpus"/> will
    /// also be deleted.
    /// If false (the default), a <c>FAILED_PRECONDITION</c> error will be returned if
    /// <see cref="V1Beta.Corpora.Corpus"/> contains any <see cref="V1Beta.FileSearchStores.Document"/>s.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Empty> DeleteCorpusAsync(
        string corpus,
        bool? force = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the latest state of a long-running operation.  Clients can use this
    /// method to poll the operation result at intervals as recommended by the API
    /// service.
    /// </summary>
    /// <param name="corpus">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="operation">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Operation> GetOperationByCorpusAndOperationAsync(
        string corpus,
        string operation,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists permissions for the specific resource.
    /// </summary>
    /// <param name="corpus">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="pageSize">
    /// Optional. The maximum number of <see cref="V1Beta.Permission"/>s to return (per page).
    /// The service may return fewer permissions.
    /// If unspecified, at most 10 permissions will be returned.
    /// This method returns at most 1000 permissions per page, even if you pass
    /// larger page_size.
    /// </param>
    /// <param name="pageToken">
    /// Optional. A page token, received from a previous <c>ListPermissions</c> call.
    /// Provide the <c>page_token</c> returned by one request as an argument to the
    /// next request to retrieve the next page.
    /// When paginating, all other parameters provided to <c>ListPermissions</c>
    /// must match the call that provided the page token.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<ListPermissionsResponse> ListPermissionsByCorpusAsync(
        string corpus,
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a permission to a specific resource.
    /// </summary>
    /// <param name="corpus">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="request">
    /// Required. The permission to create.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Permission> CreatePermissionByCorpusAsync(
        string corpus,
        Permission request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets information about a specific Permission.
    /// </summary>
    /// <param name="corpus">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="permission">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Permission> GetPermissionByCorpusAndPermissionAsync(
        string corpus,
        string permission,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the permission.
    /// </summary>
    /// <param name="corpus">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="permission">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Empty> DeletePermissionByCorpusAndPermissionAsync(
        string corpus,
        string permission,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the permission.
    /// </summary>
    /// <param name="corpus">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="permission">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="request">
    /// Required. The permission to update.
    /// The permission's <c>name</c> field is used to identify the permission to update.
    /// </param>
    /// <param name="updateMask">
    /// Required. The list of fields to update. Accepted ones:
    /// - role (<c>Permission.role</c> field)
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Permission> UpdatePermissionByCorpusAndPermissionAsync(
        string corpus,
        string permission,
        Permission request,
        string updateMask,
        CancellationToken cancellationToken = default);

}
