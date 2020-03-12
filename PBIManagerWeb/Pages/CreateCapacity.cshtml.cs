using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.Azure;
using Models.Capacity;
using PBIManagerWeb.Contracts;
using PBIManagerWeb.Models;

namespace PBIManagerWeb.Pages
{
    public class CreateModel : PageModel
    {

        private readonly ICapacityService _capacityService;
        private readonly IAzureService _azureService;

        public CreateModel(ICapacityService capacityService, IAzureService azureService)
        {
            _capacityService = capacityService;
            _azureService = azureService;
        }

        [BindProperty]
        public PBIResource CurrentResource { get; set; }

        public SelectList ResourceGroups { get; set; }
        public SelectList Skus { get; set; }
        public SelectList Regions { get; set; }

        public async Task OnGetAsync()
        {
            // fetch resource groups
            List<ResourceGroup> azureResourceGroups = new List<ResourceGroup>();

            //Convert and bind to the view model
            azureResourceGroups = await _azureService.ResourceGroups();

            //Convert and bind to the view model
            ResourceGroups = new SelectList(azureResourceGroups, "Name", "Name");

            // fetch regions
            var pbiSkus = await _capacityService.ListSkus();

            //Create a unique list based on the regions
            var regions = pbiSkus.GroupBy(r => new { r.LocationName })
                .Select(grp => grp.Key)
                .ToList();

            Regions = new SelectList(regions, "LocationName", "LocationName");


            // fetch sku sizes
            var skus = pbiSkus.GroupBy(r => r.Name)
                .Select(grp => grp.First())
                .ToList();

            Skus = new SelectList(skus, "Name", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                //Post and return to the page
                return Page();
            }


            var request = new CreateCapacity
            {
                ResourceGroupName = CurrentResource.ResourceGroupId,
                Name = CurrentResource.Name,
                Sku = new CapacitySku() { Name = CurrentResource.SkuId, Tier = "PBIE_Azure" },
                Properties = new CapacityProperties()
                {
                    Administration = new CapacityAdministrators()
                    {
                        Members = new string[]
                        {
                            CurrentResource.CapacityAdministrator
                        }
                    }
                },
                Location = CurrentResource.RegionId
            };

            await _capacityService.CreateAsync(request);

            return RedirectToPage("./ManageCapacity");
        }
    }
}