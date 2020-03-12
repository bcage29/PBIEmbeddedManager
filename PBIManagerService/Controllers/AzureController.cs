using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Azure;
using PBIManagerService.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PBIManagerService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AzureController : ControllerBase
    {
        private readonly IAzureService _azureService;

        public AzureController(IAzureService azureService)
        {
            _azureService = azureService;
        }

        /// <summary>
        /// Uses the Azure Fluent API to fetch the Azure resource groups
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ResourceGroup>> ResourceGroups()
        {
            //Get the avaiable resourceGroups for PBI
            var resourceGroups = await _azureService.GetAzureResourceGroups();
            resourceGroups.Sort((c, d) => c.Name.CompareTo(d.Name));
            return resourceGroups;
        }
    }
}