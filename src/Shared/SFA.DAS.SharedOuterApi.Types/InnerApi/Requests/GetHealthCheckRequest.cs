using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

public class GetHealthCheckRequest : IGetApiRequest
{
    public string GetUrl => "api/healthcheck";
}