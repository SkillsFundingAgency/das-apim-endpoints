using MediatR;
using SFA.DAS.EmployerAan.Infrastructure;

namespace SFA.DAS.EmployerAan.Application.MemberNotificationSettings.Queries.GetMemberNotificationSettings;

public class GetMemberNotificationSettingsQueryHandler : IRequestHandler<GetMemberNotificationSettingsQuery, GetMemberNotificationSettingsQueryResult>
{
    private readonly IAanHubRestApiClient _apiClient;

    public GetMemberNotificationSettingsQueryHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetMemberNotificationSettingsQueryResult> Handle(GetMemberNotificationSettingsQuery request, CancellationToken cancellationToken)
    {
        var eventFormats = _apiClient.GetMemberNotificationEventFormat(request.MemberId, cancellationToken);
        var locations = _apiClient.GetMemberNotificationLocations(request.MemberId, cancellationToken);
        var memberResponse = _apiClient.GetMember(request.MemberId, cancellationToken);

        await Task.WhenAll(eventFormats, locations, memberResponse);

        GetMemberNotificationSettingsQueryResult result = new();

        var eventFormatsTask = eventFormats.Result;
        var locationsTask = locations.Result;
        var outputMember = memberResponse.Result;

        if (locationsTask.MemberNotificationLocations.Any())
            result.MemberNotificationLocations = locationsTask.MemberNotificationLocations;


        if (locationsTask.MemberNotificationLocations.Any())
            result.MemberNotificationLocations = locationsTask.MemberNotificationLocations;

        result.ReceiveMonthlyNotifications = outputMember.ReceiveNotifications ?? false;

        return result;
    }
}
