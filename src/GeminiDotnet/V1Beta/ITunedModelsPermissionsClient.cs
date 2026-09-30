
namespace GeminiDotnet.V1Beta;

public partial interface ITunedModelsPermissionsClient
{
    /// <summary>
    /// Lists permissions for the specific resource.
    /// </summary>
    /// <param name="parent">
    /// Required. The parent resource of the permissions. Formats: <c>tunedModels/{tuned_model}</c> <c>corpora/{corpus}</c>
    /// A resource name of the form <c>tunedModels/{tunedModelId}</c>.
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
    Task<ListPermissionsResponse> ListAsync(
        string parent,
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a permission to a specific resource.
    /// </summary>
    /// <param name="parent">
    /// Required. The parent resource of the <see cref="V1Beta.Permission"/>. Formats: <c>tunedModels/{tuned_model}</c> <c>corpora/{corpus}</c>
    /// A resource name of the form <c>tunedModels/{tunedModelId}</c>.
    /// </param>
    /// <param name="request">Required. The permission to create.</param>
    /// <param name="cancellationToken"></param>
    Task<Permission> CreateAsync(
        string parent,
        Permission request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets information about a specific Permission.
    /// </summary>
    /// <param name="name">
    /// Required. The resource name of the permission. Formats: <c>tunedModels/{tuned_model}/permissions/{permission}</c> <c>corpora/{corpus}/permissions/{permission}</c>
    /// A resource name of the form <c>tunedModels/{tunedModelId}/permissions/{permissionId}</c>.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Permission> GetAsync(
        string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the permission.
    /// </summary>
    /// <param name="name">
    /// Required. The resource name of the permission. Formats: <c>tunedModels/{tuned_model}/permissions/{permission}</c> <c>corpora/{corpus}/permissions/{permission}</c>
    /// A resource name of the form <c>tunedModels/{tunedModelId}/permissions/{permissionId}</c>.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Empty> DeleteAsync(
        string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the permission.
    /// </summary>
    /// <param name="name">
    /// Output only. Identifier. The permission name. A unique name will be generated on create. Examples: tunedModels/{tuned_model}/permissions/{permission} corpora/{corpus}/permissions/{permission} Output only.
    /// A resource name of the form <c>tunedModels/{tunedModelId}/permissions/{permissionId}</c>.
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
    Task<Permission> PatchAsync(
        string name,
        Permission request,
        string updateMask,
        CancellationToken cancellationToken = default);

}
