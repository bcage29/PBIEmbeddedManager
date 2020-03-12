using Microsoft.Extensions.Options;
using Models.Workspace;
using Newtonsoft.Json;
using PBIManagerWeb.Common;
using PBIManagerWeb.Contracts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PBIManagerWeb.Services
{
    public class WorkspaceService : IWorkspaceService
    {
        private readonly HttpClient _httpClient;
        private readonly UrlsConfig _urls;

        public WorkspaceService(HttpClient httpClient, IOptions<UrlsConfig> config)
        {
            _httpClient = httpClient;
            _urls = config.Value;
        }

        public async Task<WorkspaceGroup> CreateAsync(CreateWorkspaceRequest request)
        {
            var content = HttpHelper.GetStringContent(request);
            var response = await _httpClient.PostAsync(_urls.PowerBIManager + UrlsConfig.Workspace.Create(), content);
            var responseString = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<WorkspaceGroup>(responseString);
        }

        public async Task<IList<WorkspaceGroup>> ListAsync()
        {
            var response = await _httpClient.GetAsync(_urls.PowerBIManager + UrlsConfig.Workspace.List());
            var data = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<IList<WorkspaceGroup>>(data);
            }

            throw new Exception(data);
        }

        public async Task<WorkspaceGroup> GetAsync(WorkspaceGroup request)
        {
            var response = await _httpClient.GetAsync(_urls.PowerBIManager + UrlsConfig.Workspace.Get(request.Id));
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<WorkspaceGroup>(responseString);
        }

        public async Task PostImportWithFileAsyncInGroup(UploadFileRequest request)
        {
            var content = HttpHelper.GetStringContent(request);
            var response = await _httpClient.PostAsync(_urls.PowerBIManager + UrlsConfig.Workspace.Upload(), content);

            response.EnsureSuccessStatusCode();
            return;
        }

        public async Task UpdateDatasetParameters(Guid groupId)
        {
            var response = await _httpClient.GetAsync(_urls.PowerBIManager + UrlsConfig.Workspace.UpdateDatasetParameters(groupId));

            response.EnsureSuccessStatusCode();
            return;
        }

        public async Task UpdateCredentials(Guid groupId)
        {
            var response = await _httpClient.GetAsync(_urls.PowerBIManager + UrlsConfig.Workspace.UpdateCredentials(groupId));

            response.EnsureSuccessStatusCode();
            return;
        }

        public async Task RefreshDataset(Guid groupId)
        {
            var response = await _httpClient.GetAsync(_urls.PowerBIManager + UrlsConfig.Workspace.RefreshDataset(groupId));

            response.EnsureSuccessStatusCode();
            return;
        }
    }
}
