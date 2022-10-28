using MediatR;

namespace SFA.DAS.Forecasting.Application.AccountUsers
{
    public class GetAccountsQuery : IRequest<GetAccountsQueryResult>
    {
        public string UserId { get; set; }
        public string Email { get; set; }
    }
}