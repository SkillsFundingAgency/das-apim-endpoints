using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ProviderCoursesService;

public class GetProviderCoursesRequest(long trainingProviderId) : IGetApiRequest
{
    public string GetUrl => $"api/providers/{trainingProviderId}/courses";
}