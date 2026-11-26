using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

public class GetEmployerAccountLegalEntityRequest(string href) : IGetApiRequest
{
    public string GetUrl => href;
}