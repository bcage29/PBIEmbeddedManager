using Microsoft.Extensions.Options;
using Models.Azure;
using Newtonsoft.Json;
using PBIManagerWeb.Common;
using PBIManagerWeb.Contracts;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PBIManagerWeb.Services
{
    public class AzureService : IAzureService
    {
        private readonly HttpClient _httpClient;
        private UrlsConfig _urls;
        public AzureService(HttpClient httpClient, IOptions<UrlsConfig> config)
        {
            _httpClient = httpClient;
            _urls = config.Value;
        }

        public async Task<List<ResourceGroup>> ResourceGroups()
        {
            var responseString = await _httpClient.GetStringAsync(_urls.PowerBIManager + UrlsConfig.Azure.ResourceGroups());

            return !string.IsNullOrEmpty(responseString)
                ? JsonConvert.DeserializeObject<List<ResourceGroup>>(responseString) : null;
        }
    }
}
