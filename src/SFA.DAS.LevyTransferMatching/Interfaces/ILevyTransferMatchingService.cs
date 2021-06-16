using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Models;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Models.Tags;

namespace SFA.DAS.LevyTransferMatching.Interfaces
{
    public interface ILevyTransferMatchingService
    {
        Task<IEnumerable<Tag>> GetLevels();
        Task<IEnumerable<Tag>> GetSectors();
        Task<IEnumerable<Tag>> GetJobRoles();
        Task<PledgeReference> CreatePledge(Pledge pledge);
        Task<IEnumerable<Pledge>> GetAllPledges();
    }
}