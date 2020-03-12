using Microsoft.PowerBI.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PBIManagerService.Contracts
{
    public interface IWorkspaceService
    {
        Task AssignWorkspaceToCapacityAsync(Guid workspaceId, Guid capacityId);

        Task<Group> CreateAsync(string workspaceName);

        Task DeleteReportAsync(Guid groupId, Guid reportId);

        Task<Group> GetByIdAsync(Guid capacityId);

        Task<IEnumerable<Group>> ListAsync();

        Task RefreshDatasetAsync(Guid groupId);

        Task UpdateDatasetParameters(Guid groupId);

        Task UpdateCredentials(Guid groupId);

        Task Upload(Guid groupId, string datasetName, string filePath, bool overwrite);
    }
}
