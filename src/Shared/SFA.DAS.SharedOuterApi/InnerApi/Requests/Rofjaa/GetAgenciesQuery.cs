using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Rofjaa;

public class GetAgenciesQuery : IGetApiRequest
{
    public string GetUrl => $"agencies";
}