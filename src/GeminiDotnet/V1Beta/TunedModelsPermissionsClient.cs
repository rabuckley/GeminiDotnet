using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace GeminiDotnet.V1Beta;

internal sealed partial class TunedModelsPermissionsClient : ITunedModelsPermissionsClient
{
    private readonly IGeminiRequester _requester;
    
    internal TunedModelsPermissionsClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<ListPermissionsResponse> ListAsync(
        string parent,
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(parent);
        if (parent.Split('/') is not ["tunedModels", { Length: > 0 }])
        {
            throw new ArgumentException($"'{parent}' is not a resource name of the form 'tunedModels/{{tunedModelId}}'.", nameof(parent));
        }
        var query = new QueryStringBuilder()
            .Add("pageSize", pageSize)
            .Add("pageToken", pageToken)
            .ToString();
        var path = $"/v1beta/{WildcardPath.Escape(parent)}/permissions{query}";
        return _requester.ExecuteAsync<ListPermissionsResponse>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Permission> CreateAsync(
        string parent,
        Permission request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(parent);
        if (parent.Split('/') is not ["tunedModels", { Length: > 0 }])
        {
            throw new ArgumentException($"'{parent}' is not a resource name of the form 'tunedModels/{{tunedModelId}}'.", nameof(parent));
        }
        ArgumentNullException.ThrowIfNull(request);
        var path = $"/v1beta/{WildcardPath.Escape(parent)}/permissions";
        return _requester.ExecuteAsync<Permission, Permission>(HttpMethod.Post, path, request, cancellationToken);
    }

    public Task<Permission> GetAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["tunedModels", { Length: > 0 }, "permissions", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'tunedModels/{{tunedModelId}}/permissions/{{permissionId}}'.", nameof(name));
        }
        var path = $"/v1beta/{WildcardPath.Escape(name)}";
        return _requester.ExecuteAsync<Permission>(HttpMethod.Get, path, cancellationToken);
    }

    public Task<Empty> DeleteAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["tunedModels", { Length: > 0 }, "permissions", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'tunedModels/{{tunedModelId}}/permissions/{{permissionId}}'.", nameof(name));
        }
        var path = $"/v1beta/{WildcardPath.Escape(name)}";
        return _requester.ExecuteAsync<Empty>(HttpMethod.Delete, path, cancellationToken);
    }

    public Task<Permission> PatchAsync(
        string name,
        Permission request,
        string updateMask,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (name.Split('/') is not ["tunedModels", { Length: > 0 }, "permissions", { Length: > 0 }])
        {
            throw new ArgumentException($"'{name}' is not a resource name of the form 'tunedModels/{{tunedModelId}}/permissions/{{permissionId}}'.", nameof(name));
        }
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(updateMask);
        var query = new QueryStringBuilder()
            .Add("updateMask", updateMask)
            .ToString();
        var path = $"/v1beta/{WildcardPath.Escape(name)}{query}";
        return _requester.ExecuteAsync<Permission, Permission>(HttpMethod.Patch, path, request, cancellationToken);
    }

}
