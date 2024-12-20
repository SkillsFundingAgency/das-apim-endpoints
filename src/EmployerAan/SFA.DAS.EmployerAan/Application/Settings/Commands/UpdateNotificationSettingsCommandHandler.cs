using MediatR;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.EmployerAan.InnerApi.Settings;

namespace SFA.DAS.EmployerAan.Application.Settings.Commands;

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
                Ordering = x.Ordering,
                ReceiveNotifications = x.ReceiveNotifications,
            }).ToList(),
            Locations = request.Locations.Select(x => new PostNotificationSettingsApiRequest.Location{
                Name = x.Name,
                Radius = x.Radius,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            }).ToList(),
        };

        await aanHubApiClient.UpdateMemberNotificationLocations(request.MemberId, updateRequest, cancellationToken);
    }
}