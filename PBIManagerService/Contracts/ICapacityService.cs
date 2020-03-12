using Microsoft.PowerBI.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PBIManagerService.Contracts
{
    public interface ICapacityService
    {
        Task AddByokToCapacity(string capacityId, string keyId);

        Task<dynamic> CheckNameAvailabilityAsync(string capacityName, string location);

        Task<dynamic> CreateAsync(string resourceGroupName, string capacityName, dynamic body);

        Task DeleteAsync(string resourceGroupName, string name);
        
        Task<dynamic> GetAsync(string resourceGroupName, string name);

        Task<Capacity> GetAsync(Guid id);

        Task<List<Capacity>> ListAsync();

        Task<dynamic> ListAzureCapacityAsync();

        Task<dynamic> ListByResourceGroupAsync(string resourceGroupName);

        Task<dynamic> ListSkusAsync();

        Task<dynamic> ListSKUsForCapacityAsync(string resourceGroupName, string capacityName);

        Task ResumeAsync(string resourceGroupName, string capacityName);

        Task SuspendAsync(string resourceGroupName, string name);

        Task<dynamic> UpdateAsync(string resourceGroupName, string name, dynamic body);
    }
}
