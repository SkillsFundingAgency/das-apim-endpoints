using MediatR;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Application.NotificationsSettings.Queries;

public class GetNotificationsSettingsQueryHandler(IAanHubRestApiClient aanHubApiClient) : IRequestHandler<GetNotificationsSettingsQuery, GetNotificationsSettingsQueryResult>
{
    public async Task<GetNotificationsSettingsQueryResult> Handle(GetNotificationsSettingsQuery request, CancellationToken cancellationToken)
    {
        var member = await aanHubApiClient.GetMember(request.MemberId, cancellationToken);

        return new GetNotificationsSettingsQueryResult
        {
            ReceiveNotifications = member.ReceiveNotifications
        };
    }
}