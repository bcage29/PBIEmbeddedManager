using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.PowerBI.Api.Models;
using Models;
using Models.Capacity;
using Newtonsoft.Json;
using PBIManagerService.Common;
using PBIManagerService.Contracts;

namespace PBIManagerService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PBICapacitiesController : ControllerBase
    {
        private readonly ILogger<PBICapacitiesController> _logger;
        private readonly ICapacityService _capacityService;
        private readonly AppConfigSettings _settings;
        private readonly IAzureService _azureService;

        public PBICapacitiesController(ILogger<PBICapacitiesController> logger, ICapacityService capacityService,
            IOptions<AppConfigSettings> settings, IAzureService azureService)
        {
            _logger = logger;
            _capacityService = capacityService;
            _settings = settings.Value;
            _azureService = azureService;
        }

        /// <summary>
        /// Check the name availability in the target location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<CapacityNameAvailable>> CheckNameAvailability(string name, string location)
        {
            var result = await _capacityService.CheckNameAvailabilityAsync(name, location);
            return JsonConvert.DeserializeObject<CapacityNameAvailable>(result);
        }

        /// <summary>
        /// Provisions the specified Dedicated capacity based on the configuration specified in the request.
        /// </summary>
        /// <param name="pBEmbeddedInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<PBIAzureCapacity>> Create(CreateCapacity req)
        {
            var admins = req.Properties.Administration.Members.ToList();
            admins.Add(_settings.ServicePrincipalOid);

            req.Properties.Administration.Members = admins.ToArray();
            dynamic body = new
            {
                location = req.Location,
                properties = req.Properties,
                sku = req.Sku,
                tags = req.Tags
            };

            var result = await _capacityService.CreateAsync(req.ResourceGroupName, req.Name, body);
            return JsonConvert.DeserializeObject<PBIAzureCapacity>(result);
        }

        /// <summary>
        /// Deletes the specified Dedicated capacity.
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(string resourceGroupName, string name)
        {
            await _capacityService.DeleteAsync(resourceGroupName, name);
            return Ok();
        }

        /// <summary>
        /// Gets details about the specified dedicated capacity using Azure REST.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PBIAzureCapacity>> GetByName(string resourceGroupName, string name)
        {
            var result = await _capacityService.GetAsync(resourceGroupName, name);
            return JsonConvert.DeserializeObject<PBIAzureCapacity>(result);
        }

        /// <summary>
        /// Gets details about the specified dedicated capacity using PBI Client.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PBIAzureCapacity>> GetById(Guid id)
        {
            var result = await _capacityService.GetAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return MapCapacityModelToDto(result);
        }

        /// <summary>
        /// Lists all the Dedicated capacities for the user using the PBI Client.
        /// User must be a dedicated capacity admin
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<PBIAzureCapacity>> List()
        {
            var result = await _capacityService.ListAsync();
            return MapCapacityModelListToDtoList(result);
        }

        /// <summary>
        /// Lists all the Dedicated capacities for the given subscription.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<PBIAzureCapacity>> ListAzureCapacityAsync()
        {
            var result = await _capacityService.ListAzureCapacityAsync();
            var capacities = JsonConvert.DeserializeObject<ODataBaseResponse<PBIAzureCapacity>>(result);
            return capacities.Value?.ToArray();
        }

        /// <summary>
        /// Gets all the Dedicated capacities for the given resource group.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<PBIAzureCapacity>> ListByResourceGroup(string resourceGroupName)
        {
            var result = await _capacityService.ListByResourceGroupAsync(resourceGroupName);
            var capacities = Newtonsoft.Json.JsonConvert.DeserializeObject<ODataBaseResponse<PBIAzureCapacity>>(result);
            return capacities.Value?.ToArray();
        }

        /// <summary>
        /// Lists eligible SKUs for PowerBI Dedicated resource provider.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<PBISkuRegion>> ListSkus()
        {
            var result = await _capacityService.ListSkusAsync();
            var skus = JsonConvert.DeserializeObject<ODataBaseResponse<PBISkuRegion>>(result);
            return skus.Value?.ToArray();
        }

        /// <summary>
        /// Lists eligible SKUs for a PowerBI Dedicated resource.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<CapacitySku>> ListSKUsForCapacity(string resourceGroupName, string name)
        {
            var result = await _capacityService.ListSKUsForCapacityAsync(resourceGroupName, name);
            ODataBaseResponse<PBIAzureCapacity> capacitySkuObj = JsonConvert.DeserializeObject<ODataBaseResponse<PBIAzureCapacity>>(result);

            //var asdf = capacitySkuObj.Value;
            var skus = capacitySkuObj.Value.Select(x => x.Sku);

            return skus;
        }

        /// <summary>
        /// Resumes a dedicated capacity
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Resume(string resourceGroupName, string name)
        {
            await _capacityService.ResumeAsync(resourceGroupName, name);
            return Ok();
        }

        /// <summary>
        /// Suspends a dedicated capacity
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Suspend(string resourceGroupName, string name)
        {
            await _capacityService.SuspendAsync(resourceGroupName, name);
            return Ok();
        }

        /// <summary>
        /// URI Parameters subscriptionId, resourceGroupName, dedicatedCapacityName, api-version
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<PBIAzureCapacity>> Update(CreateCapacity req)
        {
            dynamic body = new
            {
                properties = req.Properties,
                sku = req.Sku,
                tags = req.Tags
            };

            var result = await _capacityService.UpdateAsync(req.ResourceGroupName, req.Name, body);
            return JsonConvert.DeserializeObject<PBIAzureCapacity>(result);
        }

        [HttpPost]
        public IActionResult AddByokToCapacity(Byok req)
        {
            // User Admin Token authorization not implemented
            //await _capacityService.AddByokToCapacity(req.CapacityId, req.KeyId);
            //return Ok();

            throw new NotImplementedException();
        }


        // Map Helpers
        private List<PBIAzureCapacity> MapCapacityModelListToDtoList(List<Capacity> capacityList)
        {
            var capacityDtoList = new List<PBIAzureCapacity>();

            capacityList.ForEach(capacity => capacityDtoList
                .Add(MapCapacityModelToDto(capacity)));

            return capacityDtoList;
        }

        private PBIAzureCapacity MapCapacityModelToDto(Capacity capacity)
        {
            return new PBIAzureCapacity
            {
                Id = null,
                CapacityId = capacity.Id,
                Name = capacity.DisplayName,
                Location = capacity.Region,
                Properties = new CapacityProperties
                {
                    Administration = new CapacityAdministrators
                    {
                        Members = capacity.Admins.ToArray()
                    },
                    State = capacity.State
                },
                Sku = new CapacitySku
                {
                    Name = capacity.Sku,
                    Tier = "PBIE_AZURE"
                }
            };
        }
    }
}