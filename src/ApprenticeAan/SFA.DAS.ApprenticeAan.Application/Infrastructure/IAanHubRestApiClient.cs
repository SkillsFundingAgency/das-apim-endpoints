using RestEase;
using SFA.DAS.ApprenticeAan.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.ApprenticeAan.Application.Infrastructure.Configuration;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Attendances;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Notifications;
using SFA.DAS.ApprenticeAan.Application.Members.Queries.GetMembers;
using SFA.DAS.ApprenticeAan.Application.Models;
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

    [Post("/apprentices")]
    Task<CreateApprenticeMemberCommandResult> PostApprenticeMember([Body] CreateApprenticeMemberCommand command, CancellationToken cancellationToken);

    [Put("/CalendarEvents/{calendarEventId}/attendance")]
    Task PutAttendance(
        [Path] Guid calendarEventId,
        [Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
        [Body] AttendanceStatus putAttendanceRequest,
        CancellationToken cancellationToken);

    [Get("calendarEvents")]
    Task<GetCalendarEventsQueryResult> GetCalendarEvents(
        [Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
        [QueryMap] IDictionary<string, string[]> parameters,
        CancellationToken cancellationToken);

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

    [Get("members")]
    Task<GetMembersQueryResult> GetMembers([QueryMap] IDictionary<string, string[]> parameters, CancellationToken cancellationToken);

    [Get("/notifications/{id}")]
    Task<Response<GetNotificationResponse?>> GetNotification(
        [Path] Guid id,
        [Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
        CancellationToken cancellationToken);
}
