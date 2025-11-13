using System;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService;
using SFA.DAS.SharedOuterApi.Models.Roatp;

namespace SFA.DAS.SharedOuterApi.Interfaces;

public interface ITrainingProviderService
{
    [Obsolete("Use GetProviderDetails(int ukprn) instead")]
    Task<TrainingProviderResponse> GetTrainingProviderDetails(long ukprn);

    Task<ProviderDetailsModel> GetProviderDetails(int ukprn);
}
