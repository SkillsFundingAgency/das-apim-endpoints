using MediatR;

namespace SFA.DAS.AdminAan.Application.NotificationsSettings.Queries
{
    public class GetNotificationsSettingsQuery : IRequest<GetNotificationsSettingsQueryResult>
    {
        public Guid MemberId { get; set; }
    }
}
