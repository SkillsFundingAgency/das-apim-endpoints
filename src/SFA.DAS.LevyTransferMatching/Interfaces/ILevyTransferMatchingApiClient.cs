using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Interfaces
{
    public interface ILevyTransferMatchingApiClient
    {
        Task<bool> IsHealthy();
    }
}
