using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Workspace
{
    public class WorkspaceGroup
    {
        public WorkspaceGroup() { }

        [Display(Name = "Workspace Id"), JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [Display(Name = "Workspace Name"), JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "isReadOnly")]
        public bool? IsReadOnly { get; set; }
        [JsonProperty(PropertyName = "isOnDedicatedCapacity")]
        public bool? IsOnDedicatedCapacity { get; set; }
        [JsonProperty(PropertyName = "capacityId")]
        public Guid? CapacityId { get; set; }
        public string Type { get; set; }
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
    }
}