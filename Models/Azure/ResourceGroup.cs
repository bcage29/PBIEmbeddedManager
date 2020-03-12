namespace Models.Azure
{
    public class ResourceGroup
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string Location { get; set; }
        
        public string LocationId { get; set; }
        
        public string ResourceID { get; set; }

        public string SubscriptionId { get; set; }
    }
}
