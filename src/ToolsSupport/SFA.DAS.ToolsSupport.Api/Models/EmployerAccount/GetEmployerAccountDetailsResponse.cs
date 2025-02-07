using SFA.DAS.ToolsSupport.Application.Queries.EmployerAccount.GetEmployerAccountDetails;

namespace SFA.DAS.ToolsSupport.Api.Models.EmployerAccount;

public class GetEmployerAccountDetailsResponse
{
    public required GetEmployerAccountDetailsResult Account { get; set; }

    public static explicit operator GetEmployerAccountDetailsResponse(GetEmployerAccountDetailsResult source)
    {
        if (source == null) return new GetEmployerAccountDetailsResponse { Account = new GetEmployerAccountDetailsResult() };

        return new GetEmployerAccountDetailsResponse
        {
            Account = source
        };
    }
}
