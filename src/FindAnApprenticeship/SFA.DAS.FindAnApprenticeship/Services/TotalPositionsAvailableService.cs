using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Services
{
    public interface ITotalPositionsAvailableService
    {
        Task<long> GetTotalPositionsAvailable();
    }

    public class TotalPositionsAvailableService(
        IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        ICacheStorageService cacheStorageService) : ITotalPositionsAvailableService
    {
        public async Task<long> GetTotalPositionsAvailable()
        {
            throw new NotImplementedException();
        }
    }
}
