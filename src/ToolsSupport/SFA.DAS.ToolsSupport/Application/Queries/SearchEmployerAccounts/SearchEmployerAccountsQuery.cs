using MediatR;

namespace SFA.DAS.ToolsSupport.Application.Queries.SearchEmployerAccounts;
public class SearchEmployerAccountsQuery : IRequest<SearchEmployerAccountsQueryResult>
{
    public long? AccountId { get; set; }
    public string? PayeSchemeRef { get; set; }
    public string? EmployerName { get; set; }
}