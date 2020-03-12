using System;
using System.Collections.Generic;

namespace Models.Capacity
{
    public class PBIAzureCapacity
    {
        public string Id { get; set; }

        public Guid CapacityId { get; set; }

        public string Location { get; set; }

        public string Name { get; set; }

        public CapacityProperties Properties { get; set; }

        public CapacitySku Sku { get; set; }

        public string Type { get; set; }

        public IDictionary<string, string> Tags { get; set; }
    }
}
