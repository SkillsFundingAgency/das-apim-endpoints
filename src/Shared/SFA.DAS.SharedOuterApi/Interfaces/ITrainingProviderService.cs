using SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface ITrainingProviderService
    {
        public Task<TrainingProviderResponse> GetTrainingProviderDetails(long trainingProviderId);
    }
}
