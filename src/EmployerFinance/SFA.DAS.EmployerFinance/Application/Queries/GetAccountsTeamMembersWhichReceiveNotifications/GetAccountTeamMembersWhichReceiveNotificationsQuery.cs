using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetAccountTeamMembersWhichReceiveNotifications
{
    public class GetAccountTeamMembersWhichReceiveNotificationsQuery : IRequest<GetAccountTeamMembersWhichReceiveNotificationsQueryResult>
    {
        public long AccountId { get; set; }
    }
}
