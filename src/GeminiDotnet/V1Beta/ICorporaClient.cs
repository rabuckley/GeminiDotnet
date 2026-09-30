using GeminiDotnet.V1Beta.Corpora;

namespace GeminiDotnet.V1Beta;

public partial interface ICorporaClient
{
    /// <summary>
    /// Provides access to the Operations API operations.
    /// </summary>
    ICorporaOperationsClient Operations { get; }

    /// <summary>
    /// Provides access to the Permissions API operations.
    /// </summary>
    ICorporaPermissionsClient Permissions { get; }

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
    [Obsolete]
    Task<ListCorporaResponse> ListAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates an empty <see cref="V1Beta.Corpora.Corpus"/>.
    /// </summary>
    /// <param name="request">Required. The <see cref="V1Beta.Corpora.Corpus"/> to create.</param>
    /// <param name="cancellationToken"></param>
    [Obsolete]
    Task<Corpus> CreateAsync(
        Corpus request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets information about a specific <see cref="V1Beta.Corpora.Corpus"/>.
    /// </summary>
    /// <param name="name">
    /// Required. The name of the <see cref="V1Beta.Corpora.Corpus"/>. Example: <c>corpora/my-corpus-123</c>
    /// A resource name of the form <c>corpora/{corpusId}</c>.
    /// </param>
    /// <param name="cancellationToken"></param>
    [Obsolete]
    Task<Corpus> GetAsync(
        string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a <see cref="V1Beta.Corpora.Corpus"/>.
    /// </summary>
    /// <param name="name">
    /// Required. The resource name of the <see cref="V1Beta.Corpora.Corpus"/>. Example: <c>corpora/my-corpus-123</c>
    /// A resource name of the form <c>corpora/{corpusId}</c>.
    /// </param>
    /// <param name="force">
    /// Optional. If set to true, any <see cref="V1Beta.FileSearchStores.Document"/>s and objects related to this <see cref="V1Beta.Corpora.Corpus"/> will
    /// also be deleted.
    /// If false (the default), a <c>FAILED_PRECONDITION</c> error will be returned if
    /// <see cref="V1Beta.Corpora.Corpus"/> contains any <see cref="V1Beta.FileSearchStores.Document"/>s.
    /// </param>
    /// <param name="cancellationToken"></param>
    [Obsolete]
    Task<Empty> DeleteAsync(
        string name,
        bool? force = null,
        CancellationToken cancellationToken = default);

}
