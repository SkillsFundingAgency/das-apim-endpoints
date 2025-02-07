using MediatR;
using SFA.DAS.ToolsSupport.Models.Constants;

namespace SFA.DAS.ToolsSupport.Application.Queries.EmployerAccount.GetEmployerAccountDetails;

public class GetEmployerAccountDetailsQuery : IRequest<GetEmployerAccountDetailsResult>
{
    public long AccountId { get; set; }
    public AccountFieldSelection SelectedField { get; set; } = AccountFieldSelection.EmployerAccount;
}
