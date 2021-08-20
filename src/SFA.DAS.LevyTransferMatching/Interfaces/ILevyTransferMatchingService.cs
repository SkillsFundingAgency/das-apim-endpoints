using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Responses;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using GetAccountRequest = SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests.GetAccountRequest;

namespace SFA.DAS.LevyTransferMatching.Interfaces
{
    public interface ILevyTransferMatchingService
    {
        Task<IEnumerable<ReferenceDataItem>> GetLevels();
        Task<IEnumerable<ReferenceDataItem>> GetSectors();
        Task<IEnumerable<ReferenceDataItem>> GetJobRoles();
        Task<CreatePledgeResponse> CreatePledge(CreatePledgeRequest pledge);
		Task<IEnumerable<Pledge>> GetPledges();
        Task<GetAccountResponse> GetAccount(GetAccountRequest request);
        Task CreateAccount(CreateAccountRequest request);
        Task<Pledge> GetPledge(int id);
        Task<CreateApplicationResponse> CreateApplication(CreateApplicationRequest request);
        Task<GetApplicationResponse> GetApplication(GetApplicationRequest request);
        Task<GetApplicationsResponse> GetApplications(GetApplicationsRequest request);
    }
}