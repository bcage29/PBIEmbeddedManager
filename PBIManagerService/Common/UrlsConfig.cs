namespace PBIManagerService.Common
{
    public class UrlsConfig
    {
        /// <summary>
        /// https://docs.microsoft.com/en-us/rest/api/power-bi-embedded/
        /// </summary>
        public class PowerBIEmbedded
        {
            private static string PBIApiVersion = "2017-10-01";
            public static string CheckNameAvailability(string subscriptionId, string location) 
                => $"https://management.azure.com/subscriptions/{subscriptionId}/providers/Microsoft.PowerBIDedicated/locations/{location}/checkNameAvailability?api-version=" + PBIApiVersion;
            public static string Create(string subscriptionId, string rg, string capacityName) => 
                $"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{rg}/providers/Microsoft.PowerBIDedicated/capacities/{capacityName}?api-version=" + PBIApiVersion;
            public static string Delete(string subscriptionId, string rg, string capacityName) => 
                $"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{rg}/providers/Microsoft.PowerBIDedicated/capacities/{capacityName}?api-version=" + PBIApiVersion;
            public static string GetDetails(string subscriptionId, string rg, string capacityName) => 
                $"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{rg}/providers/Microsoft.PowerBIDedicated/capacities/{capacityName}?api-version=" + PBIApiVersion;
            public static string List(string subscriptionId) => 
                $"https://management.azure.com/subscriptions/{subscriptionId}/providers/Microsoft.PowerBIDedicated/capacities?api-version=" + PBIApiVersion;
            public static string ListByResourceGroup(string subscriptionId, string rg) => 
                $"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{rg}/providers/Microsoft.PowerBIDedicated/capacities?api-version=" + PBIApiVersion;
            public static string ListSkus(string subscriptionId) => 
                $"https://management.azure.com/subscriptions/{subscriptionId}/providers/Microsoft.PowerBIDedicated/skus?api-version=" + PBIApiVersion;
            public static string ListSkusForCapacity(string subscriptionId, string rg, string capacityName) => 
                $"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{rg}/providers/Microsoft.PowerBIDedicated/capacities/{capacityName}/skus?api-version=" + PBIApiVersion;
            public static string Resume(string subscriptionId, string rg, string capacityName) => 
                $"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{rg}/providers/Microsoft.PowerBIDedicated/capacities/{capacityName}/resume?api-version=" + PBIApiVersion;
            public static string Suspend(string subscriptionId, string rg, string capacityName) => 
                $"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{rg}/providers/Microsoft.PowerBIDedicated/capacities/{capacityName}/suspend?api-version=" + PBIApiVersion;
            public static string Update(string subscriptionId, string rg, string capacityName) => 
                $"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{rg}/providers/Microsoft.PowerBIDedicated/capacities/{capacityName}?api-version=" + PBIApiVersion;
        }
    }
}
