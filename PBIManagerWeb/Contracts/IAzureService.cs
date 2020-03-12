using Models.Azure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PBIManagerWeb.Contracts
{
    public interface IAzureService
    {
        Task<List<ResourceGroup>> ResourceGroups();
    }
}