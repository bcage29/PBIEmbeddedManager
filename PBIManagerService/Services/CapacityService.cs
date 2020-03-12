using Microsoft.Extensions.Options;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using PBIManagerService.Common;
using PBIManagerService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PBIManagerService.Services
{
    public class CapacityService : ICapacityService
    {
        private readonly HttpClient _httpClient;
        private readonly IPBIService _pbiService;
        private readonly AppConfigSettings _settings;

        public CapacityService(HttpClient httpClient, IOptions<AppConfigSettings> settings, IPBIService pbiService)
        {
            _httpClient = httpClient;
            _pbiService = pbiService;
            _settings = settings.Value;
        }

       /*
        * 
        * Power BI Embedded Dedicated Capacities are managed in Azure
        * But some operations are supported via the Power BI REST API
        * 
        * This is the reason why both sets of APIs are used
        *
        * We can fetch capacites in 2 ways: 
        * 1 - Azure REST API (must have access to the resource in Azure)
        * 2 - PBI Client (must be a dedicated capacity administrator in Power BI Portal)
        * 
        * NOTE: Azure REST API and Power BI REST API return different scheams
        * The Azure REST API returns the resource uri as the capacity Id
        * Power BI REST API returns the capacity Id as a GUID
        * 
        */


        /*
         * 
         * Azure Power BI Rest Requests
         * https://docs.microsoft.com/en-us/rest/api/power-bi-embedded/
         * 
         */

        #region Azure Power BI REST Requests
        public async Task<dynamic> CheckNameAvailabilityAsync(string capacityName, string location)
        {
            var body = new
            {
                name = capacityName,
                type = "Microsoft.PowerBIDedicated/capacities"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, UrlsConfig.PowerBIEmbedded.CheckNameAvailability(_settings.SubscriptionId, location));
            request.Content = HttpHelper.GetStringContent(body);

            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<dynamic> CreateAsync(string resourceGroupName, string capacityName, dynamic body)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, UrlsConfig.PowerBIEmbedded.Create(_settings.SubscriptionId, resourceGroupName, capacityName));
            request.Content = HttpHelper.GetStringContent(body);

            var response = await _httpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(result);
            }
            return result;
        }

        public async Task DeleteAsync(string resourceGroupName, string capacityName)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, UrlsConfig.PowerBIEmbedded.Delete(_settings.SubscriptionId, resourceGroupName, capacityName));

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return;
        }

        public async Task<dynamic> GetAsync(string resourceGroupName, string capacityName)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, UrlsConfig.PowerBIEmbedded.GetDetails(_settings.SubscriptionId, resourceGroupName, capacityName));

            var response = await _httpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(result);
            }
            return result;
        }

        public async Task<dynamic> ListAzureCapacityAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, UrlsConfig.PowerBIEmbedded.List(_settings.SubscriptionId));

            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<dynamic> ListByResourceGroupAsync(string resourceGroupName)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, UrlsConfig.PowerBIEmbedded.ListByResourceGroup(_settings.SubscriptionId, resourceGroupName));

            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<dynamic> ListSkusAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, UrlsConfig.PowerBIEmbedded.ListSkus(_settings.SubscriptionId));
            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<dynamic> ListSKUsForCapacityAsync(string resourceGroupName, string capacityName)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, UrlsConfig.PowerBIEmbedded.ListSkusForCapacity(_settings.SubscriptionId, resourceGroupName, capacityName));
            var response = await _httpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(result);
            }
            return result;
        }

        public async Task ResumeAsync(string resourceGroupName, string capacityName)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, UrlsConfig.PowerBIEmbedded.Resume(_settings.SubscriptionId, resourceGroupName, capacityName));
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return;
        }

        public async Task SuspendAsync(string resourceGroupName, string capacityName)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, UrlsConfig.PowerBIEmbedded.Suspend(_settings.SubscriptionId, resourceGroupName, capacityName));
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return;
        }

        public async Task<dynamic> UpdateAsync(string resourceGroupName, string capacityName, dynamic body)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, UrlsConfig.PowerBIEmbedded.Update(_settings.SubscriptionId, resourceGroupName, capacityName));
            request.Content = HttpHelper.GetStringContent(body);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Add an encryption to to a capacity
        /// The token must be an ADMIN USER token *not implemented*
        /// </summary>
        /// <param name="capacityId"></param>
        /// <param name="keyId"></param>
        /// <returns></returns>
        public async Task AddByokToCapacity(string capacityId, string keyId)
        {
            dynamic body = new
            {
                tenantKeyId = keyId
            };
            string url = $"https://api.powerbi.com/v1.0/myorg/admin/capacities/{capacityId}";
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            request.Content = HttpHelper.GetStringContent(body);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return;
        }
        #endregion

        /*
         * 
         * Power BI SDK Requests
         * https://github.com/microsoft/PowerBI-CSharp
         * 
         * Power BI REST
         * https://docs.microsoft.com/en-us/rest/api/power-bi/
         * 
         */

        #region Power BI SDK (REST API)
        public async Task<Capacity> GetAsync(Guid id)
        {
            var capacities = await this.ListAsync();
            return capacities.Where(x => x.Id == id).SingleOrDefault();
        }

        public async Task<List<Capacity>> ListAsync()
        {
            using (var client = await _pbiService.GetPowerBIClient())
            {
                var result = await client.Capacities.GetCapacitiesAsync();
                return result.Value?.ToList() ?? new List<Capacity>();
            }
        }
        #endregion
    }
}
