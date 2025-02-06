using MediatR;
using SFA.DAS.ToolsSupport.Models.Constants;

namespace SFA.DAS.ToolsSupport.Application.Queries.EmployerAccount.GetEmployerAccountDetails;

public class GetEmployerAccountDetailsQuery : IRequest<GetEmployerAccountDetailsResult>
{
    public string AccountHashedId { get; set; } = "";
    public string SelectedField { get; set; } = AccountFieldSelection.EmployerAccount;
}
