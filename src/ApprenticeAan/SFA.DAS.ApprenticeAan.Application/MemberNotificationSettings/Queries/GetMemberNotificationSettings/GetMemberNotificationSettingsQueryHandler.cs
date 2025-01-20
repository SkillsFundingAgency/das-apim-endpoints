﻿using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAan.Application.MemberNotificationSettings.Queries.GetMemberNotificationSettings;

public class GetMemberNotificationSettingsQueryHandler : IRequestHandler<GetMemberNotificationSettingsQuery, GetMemberNotificationSettingsQueryResult>
{
    private readonly IAanHubRestApiClient _apiClient;
    public GetMemberNotificationSettingsQueryHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    public async Task<GetMemberNotificationSettingsQueryResult> Handle(GetMemberNotificationSettingsQuery request, CancellationToken cancellationToken)
    {
        var settings = _apiClient.GetMemberNotificationSettings(request.MemberId, cancellationToken);
        var memberResponse = _apiClient.GetMember(request.MemberId, cancellationToken);
        await Task.WhenAll(settings, memberResponse);
        GetMemberNotificationSettingsQueryResult result = new();
        var settingsTask = settings.Result;
        var outputMember = memberResponse.Result;
        if (settingsTask.MemberNotificationEventFormats.Any())
            result.MemberNotificationEventFormats = settingsTask.MemberNotificationEventFormats;
        if (settingsTask.MemberNotificationLocations.Any())
            result.MemberNotificationLocations = settingsTask.MemberNotificationLocations;
        result.ReceiveMonthlyNotifications = outputMember.ReceiveNotifications;
        return result;
    }
}