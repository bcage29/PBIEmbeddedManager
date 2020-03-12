using Microsoft.Extensions.Options;
using Models.Capacity;
using Newtonsoft.Json;
using PBIManagerWeb.Common;
using PBIManagerWeb.Contracts;
using PBIManagerWeb.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PBIManagerWeb.Services
{
    public class CapacityService : ICapacityService
    {
        private readonly HttpClient _httpClient;
        private readonly UrlsConfig _urls;

        public CapacityService(HttpClient httpClient, IOptions<UrlsConfig> config)
        {
            _httpClient = httpClient;
            _urls = config.Value;
        }

        public async Task<IList<PBISkuRegion>> ListSkus()
        {
            var responseString = await _httpClient.GetStringAsync(_urls.PowerBIManager + UrlsConfig.Capacity.ListSkus());
            return JsonConvert.DeserializeObject<IList<PBISkuRegion>>(responseString);
        }

        public async Task<PBIAzureCapacity> CreateAsync(CreateCapacity request)
        {
            var content = HttpHelper.GetStringContent(request);
            var response = await _httpClient.PostAsync(_urls.PowerBIManager + UrlsConfig.Capacity.Create(), content);
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PBIAzureCapacity>(responseString);
        }

        // List with Power BI Client
        public async Task<IList<PBIAzureCapacity>> ListAsync()
        {
            var responseString = await _httpClient.GetStringAsync(_urls.PowerBIManager + UrlsConfig.Capacity.List());
            return JsonConvert.DeserializeObject<IList<PBIAzureCapacity>>(responseString);
        }

        // List with Azure REST
        public async Task<IList<PBIAzureCapacity>> ListAzureCapacityAsync()
        {
            var responseString = await _httpClient.GetStringAsync(_urls.PowerBIManager + UrlsConfig.Capacity.ListAzureCapacityAsync());
            return JsonConvert.DeserializeObject<IList<PBIAzureCapacity>>(responseString);
        }

        public async Task<PBICapacity> GetAsync(string resourceGroupName, string name)
        {
            var response = await _httpClient.GetAsync(_urls.PowerBIManager + UrlsConfig.Capacity.GetByName(resourceGroupName, name));
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var capacityResponse = JsonConvert.DeserializeObject<PBIAzureCapacity>(responseString);

            return new PBICapacity
            {
                Name = capacityResponse.Name,
                Id = capacityResponse.Id,
                Region = capacityResponse.Location,
                ResourceGroup = resourceGroupName,
                Sku = capacityResponse.Sku.Name,
                Status = capacityResponse.Properties.State == "Succeeded" ? "Active" : capacityResponse.Properties.State,
                Administrators = capacityResponse.Properties.Administration.Members
            };
        }

        public async Task<IList<CapacitySku>> ListSKUsForCapacityAsync(string resourceGroupName, string name)
        {
            var response = await _httpClient.GetAsync(_urls.PowerBIManager + UrlsConfig.Capacity.ListSkusForCapacity(resourceGroupName, name));
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IList<CapacitySku>>(responseString);
        }

        public async Task<PBIAzureCapacity> UpdateAsync(CreateCapacity request)
        {
            var content = HttpHelper.GetStringContent(request);
            var response = await _httpClient.PostAsync(_urls.PowerBIManager + UrlsConfig.Capacity.Update(), content);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PBIAzureCapacity>(responseString);
        }

        public async Task DeleteAsync(string resourceGroupName, string name)
        {
            var response = await _httpClient.DeleteAsync(_urls.PowerBIManager + UrlsConfig.Capacity.Delete(resourceGroupName, name));

            response.EnsureSuccessStatusCode();
            return;
        }

        public async Task ResumeAsync(string resourceGroupName, string name)
        {
            var response = await _httpClient.GetAsync(_urls.PowerBIManager + UrlsConfig.Capacity.Resume(resourceGroupName, name));

            response.EnsureSuccessStatusCode();
            return;
        }

        public async Task SuspendAsync(string resourceGroupName, string name)
        {
            var response = await _httpClient.GetAsync(_urls.PowerBIManager + UrlsConfig.Capacity.Suspend(resourceGroupName, name));

            response.EnsureSuccessStatusCode();
            return;
        }
    }
}
