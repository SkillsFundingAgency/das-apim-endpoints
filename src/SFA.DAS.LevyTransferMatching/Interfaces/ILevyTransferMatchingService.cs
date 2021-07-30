using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Responses;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using GetAccountRequest = SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests.GetAccountRequest;
using Pledge = SFA.DAS.LevyTransferMatching.Models.Pledge;

namespace SFA.DAS.LevyTransferMatching.Interfaces
{
    public interface ILevyTransferMatchingService
    {
        Task<IEnumerable<ReferenceDataItem>> GetLevels();
        Task<IEnumerable<ReferenceDataItem>> GetSectors();
        Task<IEnumerable<ReferenceDataItem>> GetJobRoles();
        Task<PledgeReference> CreatePledge(Pledge pledge);
		Task<GetPledgesResponse> GetPledges(GetPledgesRequest getPledgesRequest);
        Task<GetAccountResponse> GetAccount(GetAccountRequest request);
        Task CreateAccount(CreateAccountRequest request);
        Task<Pledge> GetPledge(int id);
    }
}