using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using PBIManagerWeb.Contracts;
using PBIManagerWeb.Models;

namespace PBIManagerWeb.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly ICapacityService _capacityService;

        public DetailsModel(ICapacityService capacityService)
        {
            _capacityService = capacityService;
        }

        public PBICapacity Capacity { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var resourceIdParts = Common.Utils.ParseAzureResourceId(id);

            Capacity = await _capacityService.GetAsync(resourceIdParts[4], resourceIdParts[8]);

            if (Capacity == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
