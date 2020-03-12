namespace Models.Capacity
{
    public class CapacityProperties
    {
        public string ProvisioningState { get; set; }

        public string State { get; set; }

        public CapacityAdministrators Administration { get; set; }
    }
}
