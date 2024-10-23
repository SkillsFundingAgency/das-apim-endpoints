using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;

namespace SFA.DAS.EmployerPR.InnerApi.Requests;
public class AcceptCreateAccountRequestBody
{
    public string? ActionedBy { get; set; }
    public required EmployerAccountSummary AccountDetails { get; set; }
    public required AccountLegalEntitySummary AccountLegalEntityDetails { get; set; }
}
