using Models.Azure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PBIManagerService.Contracts
{
    public interface IAzureService
    {
        Task<List<ResourceGroup>> GetAzureResourceGroups();
    }
}