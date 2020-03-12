using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Azure;
using PBIManagerService.Common;
using PBIManagerService.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PBIManagerService.Services
{
    public class AzureService : IAzureService
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger<AzureService> _logger;
        private readonly AppConfigSettings _settings;

        public AzureService(ITokenService tokenService, ILogger<AzureService> logger, IOptions<AppConfigSettings> settings)
        {
            _tokenService = tokenService;
            _logger = logger;
            _settings = settings.Value;
        }

        private Azure.IAuthenticated GetAzureAuthenticated()
        {
            // get subscription contributor service principal
            var spCredentials = _tokenService.GetAzureCredentialsForServicePrincipal();

            return Azure
                .Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(spCredentials);
        }

        public async Task<List<ResourceGroup>> GetAzureResourceGroups()
        {
            var authenticated = GetAzureAuthenticated().WithSubscription(_settings.SubscriptionId);

            var resourceGroups = await authenticated.ResourceGroups.ListAsync();
            var azureResourceGroups = new List<ResourceGroup>();
            foreach (var group in resourceGroups)
            {
                var azRgRoup = new ResourceGroup()
                {
                    Name = group.Name,
                    Id = group.Id,
                    Location = group.RegionName
                };
                azureResourceGroups.Add(azRgRoup);
            }

            return azureResourceGroups;
        }
    }
}
