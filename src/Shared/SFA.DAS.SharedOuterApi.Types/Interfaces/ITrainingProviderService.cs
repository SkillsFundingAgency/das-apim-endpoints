using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.TrainingProviderService;
using SFA.DAS.SharedOuterApi.Types.Models.Roatp;

namespace SFA.DAS.SharedOuterApi.Types.Interfaces;

public interface ITrainingProviderService
{
    [Obsolete("Use GetProviderDetails(int ukprn) instead")]
    Task<TrainingProviderResponse> GetTrainingProviderDetails(long ukprn);

    Task<ProviderDetailsModel> GetProviderDetails(int ukprn);
}
