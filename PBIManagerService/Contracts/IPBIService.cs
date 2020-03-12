using Microsoft.PowerBI.Api;
using System.Threading.Tasks;

namespace PBIManagerService.Contracts
{
    public interface IPBIService
    {
        Task<IPowerBIClient> GetPowerBIClient();
    }
}