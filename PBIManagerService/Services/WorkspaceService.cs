using Microsoft.Extensions.Options;
using Microsoft.PowerBI.Api.Models;
using Microsoft.PowerBI.Api;
using PBIManagerService.Common;
using PBIManagerService.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.PowerBI.Api.Models.Credentials;
using Microsoft.Extensions.Logging;

namespace PBIManagerService.Services
{
    public class WorkspaceService : IWorkspaceService
    {
        private readonly AppConfigSettings _settings;
        private readonly IPBIService _pbiService;
        private readonly ILogger<WorkspaceService> _logger;

        public WorkspaceService(IOptions<AppConfigSettings> settings, IPBIService pbiService, ILogger<WorkspaceService> logger)
        {
            _settings = settings.Value;
            _pbiService = pbiService;
            _logger = logger;
        }

        public async Task<Group> CreateAsync(string workspaceName)
        {
            using (var client = await _pbiService.GetPowerBIClient())
            {
                var groupRequest = new GroupCreationRequest(workspaceName);
                var group = await client.Groups.CreateGroupAsync(groupRequest, workspaceV2: true);

                if (string.IsNullOrEmpty(_settings.UserEmail))
                    return group;
                
                try
                {
                    var adminUser = new GroupUser()
                    {
                        GroupUserAccessRight = "Admin",
                        EmailAddress = _settings.UserEmail
                    };

                    await client.Groups.AddGroupUserAsync(group.Id, adminUser);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Unable to add admin user to new workspace - {ex.Message}");
                }
                
                return group;
            }
        }

        public async Task<Group> GetByIdAsync(Guid id)
        {
            using (var client = await _pbiService.GetPowerBIClient())
            {
                var groups = await client.Groups.GetGroupsAsync();
                return groups.Value.Where(x => x.Id == id).FirstOrDefault();
            }
        }

        public async Task DeleteReportAsync(Guid groupId, Guid reportId)
        {
            using (var client = await _pbiService.GetPowerBIClient())
            {
                await client.Reports.DeleteReportInGroupAsync(groupId, reportId);
            }
            return;
        }

        public async Task<IEnumerable<Group>> ListAsync()
        {
            using (var client = await _pbiService.GetPowerBIClient())
            {
                var result = await client.Groups.GetGroupsAsync();
                return result.Value?.ToList();
            }
        }

        public async Task Upload(Guid groupId, string datasetName, string filePath, bool overwrite)
        {
            using (var client = await _pbiService.GetPowerBIClient())
            {
                var conflict = overwrite ? ImportConflictHandlerMode.CreateOrOverwrite : ImportConflictHandlerMode.Ignore;
                using var fs = new FileStream(filePath, FileMode.Open);
                await client.Imports.PostImportWithFileAsyncInGroup(groupId, fs, datasetName, conflict);
            }

            return;
        }

        public async Task UpdateDatasetParameters(Guid groupId)
        {
            using (var client = await _pbiService.GetPowerBIClient())
            {
                var datasources = await client.Datasets.GetDatasetsInGroupAsync(groupId);
                var dataSetId = datasources.Value.First().Id;
                var gatewayresponse = await client.Datasets.GetDatasourcesInGroupAsync(groupId, dataSetId);
                var gateway = gatewayresponse.Value.First();

                // server and database name would most likely be passed in 
                // through the API in a real world scenario
                var updateParametersList = new List<UpdateMashupParameterDetails>
                {
                    { new UpdateMashupParameterDetails { Name = "servername", NewValue = _settings.DatabaseServerName } },
                    { new UpdateMashupParameterDetails { Name = "databasename", NewValue = _settings.DatabaseName } },
                };

                var paramRequest = new UpdateMashupParametersRequest(updateParametersList);
                await client.Datasets.UpdateParametersInGroupAsync(groupId, dataSetId, paramRequest);
            }

            return;
        }

        public async Task UpdateCredentials(Guid groupId)
        {
            using (var client = await _pbiService.GetPowerBIClient())
            {
                var datasources = await client.Datasets.GetDatasetsInGroupAsync(groupId);
                var dataSetId = datasources.Value.First().Id;
                var gatewayresponse = await client.Datasets.GetDatasourcesInGroupAsync(groupId, dataSetId);
                var gateway = gatewayresponse.Value.First();

                if (gateway == null)
                {
                    throw new Exception($"Gateway is null for {groupId}");
                }

                var updateRequest = new UpdateDatasourceRequest
                {
                    CredentialDetails = new CredentialDetails(
                        new BasicCredentials(_settings.DatabaseUserName, _settings.DatabasePassword),
                        PrivacyLevel.Private, 
                        EncryptedConnection.Encrypted)
                };

                try
                {
                    client.Gateways.UpdateDatasource(gateway.GatewayId.Value, gateway.DatasourceId.Value, updateRequest);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return;
        }

        public async Task RefreshDatasetAsync(Guid groupId)
        {
            using (var client = await _pbiService.GetPowerBIClient())
            {
                var datasources = await client.Datasets.GetDatasetsInGroupAsync(groupId);
                var dataSetId = datasources.Value.First().Id;
                var gatewayresponse = await client.Datasets.GetDatasourcesInGroupAsync(groupId, dataSetId);
                var gateway = gatewayresponse.Value.First();

                await client.Datasets.RefreshDatasetInGroupAsync(groupId, dataSetId);
            }

            return;
        }

        public async Task AssignWorkspaceToCapacityAsync(Guid workspaceId, Guid capacityId)
        {
            using (var client = await _pbiService.GetPowerBIClient())
            {
                // add capacity to group
                await client.Groups.AssignToCapacityAsync(
                    workspaceId,
                    new AssignToCapacityRequest(capacityId));

                return;
            }
        }
    }
}
