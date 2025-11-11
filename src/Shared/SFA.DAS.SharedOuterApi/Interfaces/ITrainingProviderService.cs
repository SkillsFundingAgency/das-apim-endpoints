using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface ITrainingProviderService
    {
        public Task<TrainingProviderResponse> GetTrainingProviderDetails(long ukprn);
    }
}
