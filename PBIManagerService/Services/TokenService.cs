using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using PBIManagerService.Common;
using PBIManagerService.Contracts;
using System.Threading.Tasks;

namespace PBIManagerService.Services
{
    public class TokenService : ITokenService
    {
        private static string TokenEndpointHost = "https://login.microsoftonline.com";
        private readonly string _tenantId;
        private readonly string _servicePrincipalId;
        private readonly string _servicePrincipalSecret;

        public TokenService(IOptions<AppConfigSettings> settings)
        {
            _tenantId = settings.Value.TenantId;
            _servicePrincipalId = settings.Value.ServicePrincipalId;
            _servicePrincipalSecret = settings.Value.ServicePrincipalKey;
        }

        /// <summary>
        /// Request a token with the Azure Management scope
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAzureAccessTokenAsync()
        {
            string[] scopes = new string[] { "https://management.azure.com/.default" };
            return await GetAppOnlyAccessToken(scopes);
        }

        /// <summary>
        /// Request a token with the PowerBI scope
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPowerBIAccessToken()
        {
            string[] scopes = new string[] { "https://analysis.windows.net/powerbi/api/.default" };
            return await GetAppOnlyAccessToken(scopes);
        }

        /// <summary>
        /// Uses Azure Fluent API 
        /// </summary>
        /// <returns></returns>
        public AzureCredentials GetAzureCredentialsForServicePrincipal()
        {
            return SdkContext.AzureCredentialsFactory
                .FromServicePrincipal(
                    _servicePrincipalId,
                    _servicePrincipalSecret,
                    _tenantId, AzureEnvironment.AzureGlobalCloud);
        }

        /// <summary>
        /// Handles OAuth2 token requests
        /// </summary>
        /// <param name="scopes"></param>
        /// <returns></returns>
        private async Task<string> GetAppOnlyAccessToken(string[] scopes)
        {
            string tenantAuthority = $"{TokenEndpointHost}/{_tenantId}";

            var app = ConfidentialClientApplicationBuilder.Create(_servicePrincipalId)
                                    .WithClientSecret(_servicePrincipalSecret)
                                    .WithAuthority(tenantAuthority)
                                    .Build();

            var authResult = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            return authResult.AccessToken;
        }
    }
}
