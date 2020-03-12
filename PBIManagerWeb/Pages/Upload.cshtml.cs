using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.Workspace;
using PBIManagerWeb.Contracts;

namespace PBIManagerWeb.Pages
{
    public class UploadModel : PageModel
    {
        private IWebHostEnvironment _environment;
        private readonly IWorkspaceService _workspaceService;

        public UploadModel(IWebHostEnvironment environment, IWorkspaceService workspaceService)
        {
            _environment = environment;
            _workspaceService = workspaceService;
        }

        [BindProperty]
        public IFormFile Upload { get; set; }

        [BindProperty]
        public WorkspaceGroup Workspace { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var workspaceRequest = new WorkspaceGroup
            {
                Id = new Guid(id)
            };

            Workspace = await _workspaceService.GetAsync(workspaceRequest);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // upload file to a temporary location on the machine
            // send the file path to the service to fetch the file
            var filePath = Path.Combine(_environment.ContentRootPath, "bin", Upload.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await Upload.CopyToAsync(fileStream);
            }

            var request = new UploadFileRequest
            {
                FilePath = filePath,
                DatasetName = Workspace.Name + " Dataset",
                GroupId = Workspace.Id,
                Overwrite = true
            };

            try
            {
                await _workspaceService.PostImportWithFileAsyncInGroup(request);
                Thread.Sleep(1000);
                await _workspaceService.UpdateDatasetParameters(request.GroupId);
                Thread.Sleep(1000);
                await _workspaceService.UpdateCredentials(request.GroupId);
                Thread.Sleep(1000);
                await _workspaceService.RefreshDataset(request.GroupId);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                System.IO.File.Delete(filePath);
            }
            
            return RedirectToPage("./ManageWorkspace");
        }
    }
}
