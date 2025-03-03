using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;

public class GetEmployerAgreementTemplatesRequest : IGetApiRequest
{
    public string GetUrl => $"api/employeragreementtemplates";
}
