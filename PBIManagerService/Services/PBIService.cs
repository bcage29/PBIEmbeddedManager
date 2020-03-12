using Microsoft.PowerBI.Api;
using Microsoft.Rest;
using PBIManagerService.Contracts;
using System;
using System.Threading.Tasks;

namespace PBIManagerService.Services
{
    public class PBIService : IPBIService
    {
        private const string urlPowerBiRestApiRoot = "https://api.powerbi.com/";
        private readonly ITokenService _tokenService;

        public PBIService(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<IPowerBIClient> GetPowerBIClient()
        {
            var tokenCredentials = new TokenCredentials(await _tokenService.GetPowerBIAccessToken(), "Bearer");
            return new PowerBIClient(new Uri(urlPowerBiRestApiRoot), tokenCredentials);
        }
    }
}
