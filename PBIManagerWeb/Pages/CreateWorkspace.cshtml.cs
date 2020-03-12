using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.Workspace;
using PBIManagerWeb.Common;
using PBIManagerWeb.Contracts;
using PBIManagerWeb.Models;

namespace PBIManagerWeb.Pages
{
    public class CreateWorkspaceModel : PageModel
    {
        private readonly IWorkspaceService _workspaceService;
        private readonly ICapacityService _capacityService;

        public CreateWorkspaceModel(IWorkspaceService workspaceService, ICapacityService capacityService)
        {
            _workspaceService = workspaceService;
            _capacityService = capacityService;
        }

        [BindProperty]
        public PBIWorkspace Workspace { get; set; }

        public SelectList Capacities { get; set; }

        public WorkspaceGroup Group { get; set; }

        public async Task OnGetAsync()
        {
            // fetch capacities
            var capacities = await _capacityService.ListAsync();
            Capacities = new SelectList(capacities.Where(x => x.Properties.State == "Active"), "Name", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                //Post and return to the page
                return Page();
            }
            // we need the resource group for updating the capacity later in the
            // create process and the pbi capacity api does not return a resource group
            var pbiCapacities = await _capacityService.ListAsync();

            // The user will need to have access to the capacity in Azure and be a dedicated capacity admin
            var pbiCapacity = pbiCapacities.Where(x => x.Name == Workspace.CapacityName).SingleOrDefault();
            if (pbiCapacity == null)
            {
                //Post and return to the page
                return Page();
            }

            //var resourceIdParts = Utils.ParseAzureResourceId(pbiCapacity.Id.ToString());
            //var resourceGroup = resourceIdParts[4];
            var request = new CreateWorkspaceRequest
            {
                CapacityId = pbiCapacity.CapacityId,
                Name = Workspace.Name
            };

            var group = await _workspaceService.CreateAsync(request);

            Group = group;

            return Page();
        }
    }
}