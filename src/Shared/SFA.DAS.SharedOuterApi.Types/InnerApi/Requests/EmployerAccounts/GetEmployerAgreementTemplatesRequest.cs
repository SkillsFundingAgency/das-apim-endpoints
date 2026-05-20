using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EmployerAccounts;

public class GetEmployerAgreementTemplatesRequest : IGetApiRequest
{
    public string GetUrl => $"api/employeragreementtemplates";
}
