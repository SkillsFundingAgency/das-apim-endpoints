using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Models;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Accounts;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;

namespace SFA.DAS.LevyTransferMatching.Interfaces
{
    public interface ILevyTransferMatchingService
    {
        Task<IEnumerable<ReferenceDataItem>> GetLevels();
        Task<IEnumerable<ReferenceDataItem>> GetSectors();
        Task<IEnumerable<ReferenceDataItem>> GetJobRoles();
        Task<int> CreatePledge(Pledge pledge);
		Task<IEnumerable<Pledge>> GetPledges();
        Task CreateAccount(CreateAccountRequest request);
    }
}