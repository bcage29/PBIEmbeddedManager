using System.Collections.Generic;

namespace Models.Capacity
{
    public class CreateCapacity
    {
        public string ResourceGroupName { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public CapacitySku Sku { get; set; }

        public IDictionary<string, string> Tags { get; set; }

        public CapacityProperties Properties { get; set; }
    }
}
