using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestEase;
using SFA.DAS.ApprenticeAan.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.ApprenticeAan.Application.Infrastructure.Configuration;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Apprentices;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Attendances;
using SFA.DAS.ApprenticeAan.Application.InnerApi.MemberProfiles;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Members;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Notifications;
using SFA.DAS.ApprenticeAan.Application.InnerApi.StagedApprentices;
using SFA.DAS.ApprenticeAan.Application.LeavingReasons.Queries.GetLeavingReasons;
using SFA.DAS.ApprenticeAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
using SFA.DAS.ApprenticeAan.Application.Members.Queries.GetMember;
using SFA.DAS.ApprenticeAan.Application.Members.Queries.GetMembers;
using SFA.DAS.ApprenticeAan.Application.Model;
using SFA.DAS.ApprenticeAan.Application.Profiles.Queries.GetProfilesByUserType;
using SFA.DAS.ApprenticeAan.Application.Regions.Queries.GetRegions;

namespace SFA.DAS.ApprenticeAan.Application.Infrastructure;

public interface IAanHubRestApiClient
{
    [Get("/regions")]
    Task<GetRegionsQueryResult> GetRegions(CancellationToken cancellationToken);

    [Get("/calendars")]
    Task<List<Calendar>> GetCalendars(CancellationToken cancellationToken);

    [Get("/profiles/{userType}")]
    Task<GetProfilesByUserTypeQueryResult> GetProfiles([Path] string userType, CancellationToken cancellationToken);

    [Get("apprentices/{ApprenticeId}")]
    [AllowAnyStatusCode]
    Task<Response<GetApprenticeResult>> GetApprenticeMember([Path] Guid ApprenticeId, CancellationToken cancellation);

    [Post("/apprentices")]
    Task<CreateApprenticeMemberCommandResult> PostApprenticeMember([Body] CreateApprenticeMemberCommand command, CancellationToken cancellationToken);

    [Put("/CalendarEvents/{calendarEventId}/attendance")]
    Task PutAttendance(
        [Path] Guid calendarEventId,
        [Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
        [Body] AttendanceStatus putAttendanceRequest,
        CancellationToken cancellationToken);

    [Get("calendarEvents")]
    Task<GetCalendarEventsQueryResult> GetCalendarEvents([Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, [QueryMap] IDictionary<string, string[]> parameters, CancellationToken cancellationToken);

    [Get("calendarEvents/{calendarEventId}")]
    [AllowAnyStatusCode]
    Task<Response<CalendarEvent>> GetCalendarEventById(
        [Path] Guid calendarEventId,
        [Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
        CancellationToken cancellationToken);

    [Get("attendances")]
    Task<GetAttendancesResponse> GetAttendances(
        [Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
        string FromDate,
        string ToDate,
        CancellationToken cancellationToken);

    [Put("members/{memberId}/profile")]
    Task PutMemberProfile(
        [Path] Guid memberId,
        [Body] UpdateMemberProfileRequest putMemberProfileRequest,
        CancellationToken cancellationToken);

    [Get("members")]
    Task<GetMembersQueryResult> GetMembers([QueryMap] IDictionary<string, string[]> parameters, CancellationToken cancellationToken);

    [Get("members/{memberId}")]
    Task<GetMemberQueryResult> GetMember([Path] Guid memberId, CancellationToken cancellationToken);

    [Get("/notifications/{id}")]
    Task<Response<GetNotificationResponse?>> GetNotification([Path] Guid id, [Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
    CancellationToken cancellationToken);

    [Get("/members/{memberId}/profile")]
    Task<GetMemberProfileWithPreferencesQueryResult> GetMemberProfileWithPreferences([Path] Guid memberId, [Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, bool @public, CancellationToken cancellationToken);

    [Post("/notifications")]
    [AllowAnyStatusCode]
    Task<Response<GetNotificationResponse>> PostNotification([Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, [Body] PostNotificationRequest command, CancellationToken cancellationToken);

    [Get("/stagedApprentices")]
    [AllowAnyStatusCode]
    Task<Response<GetStagedApprenticeResponse?>> GetStagedApprentice([FromQuery] string lastName, [FromQuery] string dateOfBirth, [FromQuery] string email, CancellationToken cancellationToken);

    [Patch("members/{memberId}")]
    Task PatchMember(
        [Path] Guid memberId,
        [Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
        [Body] JsonPatchDocument<PatchMemberRequest> patchMemberRequest,
        CancellationToken cancellationToken);

    [Get("/leavingReasons")]
    Task<GetLeavingReasonsQueryResult> GetLeavingReasons(CancellationToken cancellationToken);
}
