using MediatR;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetEmployerAccountDetails;

public class GetEmployerAccountDetailsQuery : IRequest<GetEmployerAccountDetailsResult>
{
    public long AccountId { get; set; }
}
