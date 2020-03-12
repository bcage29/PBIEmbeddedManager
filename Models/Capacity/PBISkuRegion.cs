using System.Collections.Generic;

namespace Models.Capacity
{
    public class PBISkuRegion
    {
        public string ResourceType { get; set; }

        public string Name { get; set; }

        public List<string> Locations { get; set; }

        public IList<LocationInfo> LocationInfo { get; set; }

        public IList<string> Restrictions { get; set; }

        public string LocationName
        {
            get { return Locations[0]; }
        }
    }

    public class LocationInfo
    {
        public string Location { get; set; }

        public IList<string> Zones { get; set; }
    }
}
