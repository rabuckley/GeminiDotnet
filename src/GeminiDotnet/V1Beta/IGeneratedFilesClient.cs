using GeminiDotnet.V1Beta.GeneratedFiles;

namespace GeminiDotnet.V1Beta;

public partial interface IGeneratedFilesClient
{
    /// <summary>
    /// Provides access to the Operations API operations.
    /// </summary>
    IGeneratedFilesOperationsClient Operations { get; }

    /// <summary>
    /// Lists the generated files owned by the requesting project.
    /// </summary>
    /// <param name="pageSize">
    /// Optional. Maximum number of <see cref="V1Beta.GeneratedFiles.GeneratedFile"/>s to return per page.
    /// If unspecified, defaults to 10. Maximum <c>page_size</c> is 50.
    /// </param>
    /// <param name="pageToken">Optional. A page token from a previous <c>ListGeneratedFiles</c> call.</param>
    /// <param name="cancellationToken"></param>
    Task<ListGeneratedFilesResponse> ListAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a generated file. When calling this method via REST, only the metadata
    /// of the generated file is returned. To retrieve the file content via REST,
    /// add alt=media as a query parameter.
    /// </summary>
    /// <param name="name">A resource name of the form <c>generatedFiles/{generatedFileId}</c>.</param>
    /// <param name="cancellationToken"></param>
    Task<GeneratedFile> GetAsync(
        string name,
        CancellationToken cancellationToken = default);

}
