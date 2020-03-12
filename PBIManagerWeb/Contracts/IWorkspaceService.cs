using Models.Workspace;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PBIManagerWeb.Contracts
{
    public interface IWorkspaceService
    {
        Task<IList<WorkspaceGroup>> ListAsync();
        Task PostImportWithFileAsyncInGroup(UploadFileRequest request);
        Task<WorkspaceGroup> GetAsync(WorkspaceGroup request);
        Task<WorkspaceGroup> CreateAsync(CreateWorkspaceRequest request);
        Task UpdateDatasetParameters(Guid groupId);
        Task UpdateCredentials(Guid groupId);
        Task RefreshDataset(Guid groupId);
    }
}