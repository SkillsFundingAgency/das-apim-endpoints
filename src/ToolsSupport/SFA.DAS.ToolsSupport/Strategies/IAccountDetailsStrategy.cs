using SFA.DAS.ToolsSupport.Application.Queries.GetEmployerAccountDetails;
using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Strategies;

public interface IAccountDetailsStrategy
{
    Task<GetEmployerAccountDetailsResult> ExecuteAsync(Account account);
}
