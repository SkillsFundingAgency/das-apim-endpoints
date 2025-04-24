using MediatR;

namespace SFA.DAS.Recruit.Application.AccountUsers;

public class GetAccountsQuery : IRequest<GetAccountsQueryResult>
{
    public string UserId { get; set; }
    public string Email { get; set; }
}