using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Application.MemberNotificationSettings.Queries.GetMemberNotificationSettings;

public class GetMemberNotificationSettingsQueryHandler(IAanHubRestApiClient apiClient)
    : IRequestHandler<GetMemberNotificationSettingsQuery, GetMemberNotificationSettingsQueryResult>
{
    public async Task<GetMemberNotificationSettingsQueryResult> Handle(GetMemberNotificationSettingsQuery request, CancellationToken cancellationToken)
    {
        var settings = apiClient.GetMemberNotificationSettings(request.MemberId, cancellationToken);
        var memberResponse = apiClient.GetMember(request.MemberId, cancellationToken);

        await Task.WhenAll(settings, memberResponse);

        var settingsTask = settings.Result;
        var outputMember = memberResponse.Result;

        GetMemberNotificationSettingsQueryResult result = new()
        {
            MemberNotificationEventFormats = settingsTask.EventTypes.Select(x => new MemberNotificationEventFormatModel
            {
                EventFormat = x.EventType,
                ReceiveNotifications = x.ReceiveNotifications
            }),
            MemberNotificationLocations = settingsTask.Locations.Select(x => new MemberNotificationLocationsModel
            {
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Name = x.Name,
                Radius = x.Radius
            }),
            ReceiveMonthlyNotifications = outputMember.ReceiveNotifications
        };

        return result;
    }
}