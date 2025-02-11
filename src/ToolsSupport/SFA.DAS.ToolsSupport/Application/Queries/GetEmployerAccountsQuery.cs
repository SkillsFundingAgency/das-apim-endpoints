using MediatR;

namespace SFA.DAS.ToolsSupport.Application.Queries;
public class GetEmployerAccountsQuery : IRequest<GetEmployerAccountsQueryResult>
{
    public long? AccountId { get; set; }
    public string? PayeSchemeRef { get; set; }
}