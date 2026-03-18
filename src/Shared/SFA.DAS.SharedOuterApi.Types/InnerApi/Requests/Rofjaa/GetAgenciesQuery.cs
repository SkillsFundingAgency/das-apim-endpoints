using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Rofjaa;

public class GetAgenciesQuery : IGetApiRequest
{
    public string GetUrl => $"agencies";
}