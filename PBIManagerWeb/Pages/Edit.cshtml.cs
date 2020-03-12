using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.Capacity;
using PBIManagerWeb.Contracts;
using PBIManagerWeb.Models;

namespace PBIManagerWeb.Pages
{
    public class EditModel : PageModel
    {
        private readonly ICapacityService _capacityService;

        public EditModel(ICapacityService capacityService)
        {
            _capacityService = capacityService;
        }

        [BindProperty]
        public PBICapacity Capacity { get; set; }
        public List<SelectListItem> AvailableSkus { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var resourceIdParts = Common.Utils.ParseAzureResourceId(id);
            var resourceGroupName = resourceIdParts[4];
            var capacityName = resourceIdParts[8];

            var capacity = await _capacityService.GetAsync(resourceGroupName, capacityName);
            if (capacity == null)
                return Page();

            Capacity = capacity;

            var availableSkusList = await _capacityService.ListSKUsForCapacityAsync(resourceGroupName, capacityName);

            var items = new List<SelectListItem>();
            foreach (var sku in availableSkusList)
            {
                items.Add(new SelectListItem()
                {
                    Value = sku.Name,
                    Text = sku.Name,
                    Selected = sku.Name == Capacity.Sku
                });
            }

            AvailableSkus = items;

            if (Capacity == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var updateRequest = new CreateCapacity
            {
                ResourceGroupName = Capacity.ResourceGroup,
                Name = Capacity.Name,
                Sku = new CapacitySku { Name = Capacity.Sku, Tier = "PBIE_Azure" },
            };
            var updatedCapacity = await _capacityService.UpdateAsync(updateRequest);

            return RedirectToPage("./ManageCapacity");
        }

        public async Task<IActionResult> OnPostUpdateStateAsync()
        {
            // id for the item to set the inverse state
            if (Capacity.Status == "Active")
            {
                await _capacityService.SuspendAsync(Capacity.ResourceGroup, Capacity.Name);
            }
            else
            {
                await _capacityService.ResumeAsync(Capacity.ResourceGroup, Capacity.Name);
            }

            return RedirectToPage("./ManageCapacity");
        }
    }
}
