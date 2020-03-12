using System;

namespace Models.Workspace
{
    public class CreateWorkspaceRequest
    {
        public Guid CapacityId { get; set; }

        public string Name { get; set; }
    }
}