using Models.Capacity;
using PBIManagerWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PBIManagerWeb.Contracts
{
    public interface ICapacityService
    {
        Task<IList<PBISkuRegion>> ListSkus();

        Task<PBIAzureCapacity> CreateAsync(CreateCapacity request);

        Task<IList<PBIAzureCapacity>> ListAsync();
        
        Task<IList<PBIAzureCapacity>> ListAzureCapacityAsync();

        Task<PBICapacity> GetAsync(string resourceGroupName, string name);

        Task<IList<CapacitySku>> ListSKUsForCapacityAsync(string resourceGroupName, string name);
        
        Task<PBIAzureCapacity> UpdateAsync(CreateCapacity request);
        
        Task DeleteAsync(string resourceGroupName, string name);
        
        Task ResumeAsync(string resourceGroupName, string name);
        
        Task SuspendAsync(string resourceGroupName, string name);
    }
}