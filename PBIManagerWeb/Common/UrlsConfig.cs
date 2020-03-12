using System;

namespace PBIManagerWeb.Common
{
    public class UrlsConfig
    {
        public static class Workspace
        {
            public static string Create() => "/api/PBIWorkspace/Create";
            public static string Get(Guid id) => $"/api/PBIWorkspace/Get?Id={id}";
            public static string Upload() => "/api/PBIWorkspace/Upload";
            public static string List() => "/api/PBIWorkspace/List";

            public static string UpdateDatasetParameters(Guid id) => $"/api/PBIWorkspace/UpdateDatasetParameters?Id={id}";
            public static string UpdateCredentials(Guid id) => $"/api/PBIWorkspace/UpdateCredentials?Id={id}";
            public static string RefreshDataset(Guid id) => $"/api/PBIWorkspace/RefreshDataset?Id={id}";
        }

        public static class Capacity
        {
            public static string ListSkus() => $"/api/PBICapacities/ListSkus";
            public static string Create() => $"/api/PBICapacities/Create";
            public static string List() => $"/api/PBICapacities/List";
            public static string ListAzureCapacityAsync() => $"/api/PBICapacities/ListAzureCapacity";
            public static string GetByName(string resourceGroupName, string name) => $"/api/PBICapacities/GetByName?resourceGroupName={resourceGroupName}&name={name}";
            public static string ListSkusForCapacity(string resourceGroupName, string name) => $"/api/PBICapacities/ListSKUsForCapacity?resourceGroupName={resourceGroupName}&name={name}";
            public static string Update() => $"/api/PBICapacities/Update";
            public static string Delete(string resourceGroupName, string name) => $"/api/PBICapacities/Delete?resourceGroupName={resourceGroupName}&name={name}";
            public static string Resume(string resourceGroupName, string name) => $"/api/PBICapacities/Resume?resourceGroupName={resourceGroupName}&name={name}";
            public static string Suspend(string resourceGroupName, string name) => $"/api/PBICapacities/Suspend?resourceGroupName={resourceGroupName}&name={name}";
        }

        public static class Azure
        {
            public static string ResourceGroups() => $"/api/Azure/ResourceGroups";
        }

        public string PowerBIManager { get; set; }
    }
}
