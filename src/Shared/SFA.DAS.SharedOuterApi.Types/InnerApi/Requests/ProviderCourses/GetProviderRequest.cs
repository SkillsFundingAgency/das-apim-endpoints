using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ProviderCourses;

public class GetProviderRequest(int id) : IGetApiRequest
{
    public string GetUrl => $"api/providers/{id}";
}