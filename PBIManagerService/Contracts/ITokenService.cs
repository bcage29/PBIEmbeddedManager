using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using System.Threading.Tasks;

namespace PBIManagerService.Contracts
{
    public interface ITokenService
    {
        Task<string> GetAzureAccessTokenAsync();

        Task<string> GetPowerBIAccessToken();

        AzureCredentials GetAzureCredentialsForServicePrincipal();
    }
}