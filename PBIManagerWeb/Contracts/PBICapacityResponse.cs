using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PBIManagerWeb.Contracts
{
    public class PBICapacityResponse
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName { get; set; }
        [JsonProperty(PropertyName = "admins")]
        public IList<string> Admins { get; set; }
        [JsonProperty(PropertyName = "sku")]
        public string Sku { get; set; }
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
        //[JsonProperty(PropertyName = "capacityUserAccessRight")]
        //public string CapacityUserAccessRight { get; set; }
        [JsonProperty(PropertyName = "region")]
        public string Region { get; set; }
    }
}
