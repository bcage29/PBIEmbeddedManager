using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.PowerBI.Api.Models;
using Models.Workspace;
using PBIManagerService.Contracts;

namespace PBIManagerService.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PBIWorkspaceController : ControllerBase
    {
        private readonly IWorkspaceService _workspaceService;

        public PBIWorkspaceController(IWorkspaceService workspaceService)
        {
            _workspaceService = workspaceService;
        }

        [HttpGet]
        public IActionResult Health()
        {
            return Ok();
        }

        /// <summary>
        /// Assigns a Power BI Embedded Capacity to a workspace
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AssignToCapacity(AssignToCapacity req)
        {
            await _workspaceService.AssignWorkspaceToCapacityAsync(req.GroupId, req.CapacityId);
            return Ok();
        }


        /// <summary>
        /// Provisions the specified Dedicated capacity based on the configuration specified in the request.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Group>> Create(CreateWorkspaceRequest req)
        {
            var group = await _workspaceService.CreateAsync(req.Name);

            if (req.CapacityId != Guid.Empty)
            {
                await _workspaceService.AssignWorkspaceToCapacityAsync(group.Id, req.CapacityId);
            }

            return group;
        }

        /// <summary>
        /// Retrieves a workspace
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Group>> Get(Guid id)
        {
            return await _workspaceService.GetByIdAsync(id);
        }

        /// <summary>
        /// Uploads a File to Workspace Group.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Upload(UploadFileRequest request)
        {
            // upload pbix
            await _workspaceService.Upload(request.GroupId, request.DatasetName, request.FilePath, request.Overwrite);
            return Ok();
        }

        /// <summary>
        /// Retrieves all existing Power BI workspaces in the specified workspace collection.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Group>> List()
        {
            return await _workspaceService.ListAsync();
        }

        /// <summary>
        /// Updates the dataset parameters for a workspace.
        /// This fails half the time and would need retry logic
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UpdateDatasetParameters(Guid id)
        {
            // Update datasource
            await _workspaceService.UpdateDatasetParameters(id);
            return Ok();
        }

        /// <summary>
        /// Updates the dataset credentials for a workspace.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UpdateCredentials(Guid id)
        {
            // Update credentials
            await _workspaceService.UpdateCredentials(id);
            return Ok();
        }

        /// <summary>
        /// Refreshes the Dataset for a workspace.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> RefreshDataset(Guid id)
        {
            await _workspaceService.RefreshDatasetAsync(id);
            return Ok();
        }

        /// <summary>
        /// Deletes a report in a Power BI Workspace
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteReport(DeleteReportRequest request)
        {
            await _workspaceService.DeleteReportAsync(request.GroupId, request.ReportId);
            return Ok();
        }
    }
}