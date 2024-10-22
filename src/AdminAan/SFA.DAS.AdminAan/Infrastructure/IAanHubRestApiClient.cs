﻿using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using RestEase;
using SFA.DAS.AdminAan.Application.Admins.Commands.Create;
using SFA.DAS.AdminAan.Application.Admins.Queries.GetAdminMember;
using SFA.DAS.AdminAan.Application.CalendarEvents.Commands.Create;
using SFA.DAS.AdminAan.Application.CalendarEvents.Commands.Update;
using SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.AdminAan.Application.Entities;
using SFA.DAS.AdminAan.Application.Regions.Queries.GetRegions;
using SFA.DAS.AdminAan.Domain;
using SFA.DAS.AdminAan.Domain.InnerApi.AanHubApi;
using SFA.DAS.AdminAan.Domain.InnerApi.AanHubApi.Responses;
using SFA.DAS.AdminAan.Domain.LeavingReasons;

namespace SFA.DAS.AdminAan.Infrastructure;

public interface IAanHubRestApiClient : IHealthChecker
{
    [Get("/regions")]
    Task<GetRegionsQueryResult> GetRegions(CancellationToken cancellationToken);


    [Get("/calendars")]
    Task<List<Calendar>> GetCalendars(CancellationToken cancellationToken);


    [Get("calendarEvents")]
    Task<GetCalendarEventsApiResponse> GetCalendarEvents([Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, [QueryMap] IDictionary<string, string[]> parameters, CancellationToken cancellationToken);

    [Post("calendarEvents")]
    Task<PostEventCommandResult> PostCalendarEvents([Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, [Body] PostEventCommand command, CancellationToken cancellationToken);

    [Put("calendarEvents/{calendarEventId}")]
    Task PutCalendarEvent([Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, [Path] Guid calendarEventId, [Body] PutEventCommand command, CancellationToken cancellationToken);


    [Get("calendarEvents/{calendarEventId}")]
    [AllowAnyStatusCode]
    Task<GetCalendarEventByIdApiResponse> GetCalendarEvent([Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, [Path] Guid calendarEventId, CancellationToken cancellationToken);

    [Delete("calendarEvents/{calendarEventId}")]
    [AllowAnyStatusCode]
    Task<Response<Unit>> DeleteCalendarEvent([Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, [Path] Guid calendarEventId, CancellationToken cancellationToken);

    [Put("/CalendarEvents/{calendarEventId}/eventGuests")]
    Task PutGuestSpeakers(
        [Path] Guid calendarEventId,
        [Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
        [Body] PutEventGuestsModel guests,
        CancellationToken cancellationToken);

    [Get("members")]
    Task<GetMembersResponse> GetMembers([RawQueryString] string queryString, CancellationToken cancellationToken);

    [Get("/members/{email}")]
    [AllowAnyStatusCode]
    Task<Response<GetAdminMemberResult>> GetMemberByEmail([Path] string email, CancellationToken cancellationToken);

    [Get("members/{memberId}")]
    Task<GetMemberResponse> GetMember([Path] Guid memberId, CancellationToken cancellationToken);

    [Get("/members/{memberId}/profile?public=false")]
    Task<GetMemberProfilesAndPreferencesResponse> GetMemberProfileWithPreferences([Path] Guid memberId, [Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, CancellationToken cancellationToken);

    [Post("/admins")]
    [AllowAnyStatusCode]
    Task<CreateAdminMemberCommandResult> CreateAdminMember([Body] CreateAdminMemberCommand command, CancellationToken cancellationToken);

    [Get("/profiles/{userType}")]
    Task<GetProfilesResponse> GetProfiles([Path] string userType, CancellationToken cancellationToken);

    [Post("/members/{memberId}/remove")]
    Task<string> PostMemberLeaving(
        [Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
        [Path] Guid memberId,
        [Body] PostMemberStatusModel postMemberStatusModel,
        CancellationToken cancellationToken);

    [Get("/members/{memberId}/activities")]
    Task<GetMemberActivitiesResponse> GetMemberActivities(
        [Path] Guid memberId,
        CancellationToken cancellationToken);

    [Patch("members/{memberId}")]
    Task UpdateMember([Path] Guid memberId, [Body] JsonPatchDocument<PatchMemberModel> model, CancellationToken cancellationToken);
}
