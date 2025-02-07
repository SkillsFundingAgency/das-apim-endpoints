using SFA.DAS.ToolsSupport.Application.Queries.EmployerAccount.GetEmployerAccountDetails;
using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Api.Models.EmployerAccount;

public class GetEmployerAccountDetailsResponse
{
    public long AccountId { get; set; }
    public string HashedAccountId { get; set; } = "";
    public string DasAccountName { get; set; } = "";
    public DateTime DateRegistered { get; set; }
    public string OwnerEmail { get; set; } = "";
    public IEnumerable<LegalEntity> LegalEntities { get; set; } = [];

    public static explicit operator GetEmployerAccountDetailsResponse(GetEmployerAccountDetailsResult source)
    {
        if (source == null) return new GetEmployerAccountDetailsResponse();

        return new GetEmployerAccountDetailsResponse
        {
            AccountId = source.AccountId,
            HashedAccountId = source.HashedAccountId,
            DasAccountName = source.DasAccountName,
            OwnerEmail = source.OwnerEmail,
            DateRegistered = source.DateRegistered,
            LegalEntities = source.LegalEntities
        };
    }
}
