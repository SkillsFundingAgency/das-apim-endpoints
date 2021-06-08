using SFA.DAS.LevyTransferMatching.Models;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Interfaces
{
    public interface ILevyTransferMatchingService
    {
        Task<PledgeReference> CreatePledge(Pledge pledge);
    }
}