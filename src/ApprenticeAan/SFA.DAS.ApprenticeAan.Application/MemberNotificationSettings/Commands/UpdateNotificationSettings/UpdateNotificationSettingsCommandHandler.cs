using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Settings.Requests;

namespace SFA.DAS.ApprenticeAan.Application.MemberNotificationSettings.Commands.UpdateNotificationSettings;

public class UpdateNotificationSettingsCommandHandler(IAanHubRestApiClient aanHubApiClient)
    : IRequestHandler<UpdateNotificationSettingsCommand>
{
    public async Task Handle(UpdateNotificationSettingsCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = new PostNotificationSettingsApiRequest
        {
            ReceiveNotifications = request.ReceiveNotifications,
            EventTypes = request.EventTypes.Select(x => new PostNotificationSettingsApiRequest.NotificationEventType
            {
                EventType = x.EventType,
                ReceiveNotifications = x.ReceiveNotifications,
            }).ToList(),
            Locations = request.Locations.Select(x => new PostNotificationSettingsApiRequest.Location
            {
                Name = x.Name,
                Radius = x.Radius,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            }).ToList(),
        };

        await aanHubApiClient.UpdateMemberNotificationSettings(request.MemberId, updateRequest, cancellationToken);
    }
}