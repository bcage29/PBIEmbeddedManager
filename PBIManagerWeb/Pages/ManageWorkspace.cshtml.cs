using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.Workspace;
using PBIManagerWeb.Contracts;

namespace PBIManagerWeb.Pages
{
    public class ManageWorkspaceModel : PageModel
    {
        private readonly IWorkspaceService _workspaceService;

        public ManageWorkspaceModel(IWorkspaceService workspaceService)
        {
            _workspaceService = workspaceService;
        }

        public IList<WorkspaceGroup> Workspaces { get; set; }

        public async Task OnGetAsync()
        {
            //Get the iist of resources from the sub
            //Convert and bind to the view model
            var workspaceGroups = await _workspaceService.ListAsync();

            workspaceGroups = workspaceGroups.Where(x => x.IsOnDedicatedCapacity.Value).ToList();
            Workspaces = workspaceGroups;
        }
    }
}