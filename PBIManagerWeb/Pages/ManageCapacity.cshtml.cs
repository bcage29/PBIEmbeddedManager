using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Models.Capacity;
using PBIManagerWeb.Contracts;
using PBIManagerWeb.Models;

namespace PBIManagerWeb.Pages
{
    [BindProperties]
    public class ManageModel : PageModel
    {
        private readonly ICapacityService _capacityService;

        public ManageModel(ICapacityService capacityService)
        {
            _capacityService = capacityService;
        }

        public PBIResource CurrentResource { get; set; }
        public IList<PBIAzureCapacity> PBIResouces { get; set; }

        public async Task OnGetAsync()
        {
            //Get the iist of resources from the sub
            //Convert and bind to the view model
            var capacities = await _capacityService.ListAzureCapacityAsync();
            if (capacities == null)
                capacities = new List<PBIAzureCapacity>();

            PBIResouces = capacities;
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            // id for the item to set the inverse state

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var resourceIdParts = Common.Utils.ParseAzureResourceId(id);

            await _capacityService.DeleteAsync(resourceIdParts[4], resourceIdParts[8]);

            return RedirectToPage();
        }
    }
}
