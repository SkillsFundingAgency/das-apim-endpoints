using RestEase;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEventById;
using SFA.DAS.ApprenticeAan.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEventById;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.ApprenticeAan.Application.Infrastructure.Configuration;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Attendances;
using SFA.DAS.ApprenticeAan.Application.Profiles.Queries.GetProfilesByUserType;
using SFA.DAS.ApprenticeAan.Application.Regions.Queries.GetRegions;

namespace SFA.DAS.ApprenticeAan.Application.Infrastructure;

public interface IAanHubRestApiClient
{
    [Get("/regions")]
    Task<GetRegionsQueryResult> GetRegions(CancellationToken cancellationToken);

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
    Task<GetCalendarEventsQueryResult> GetCalendarEvents([Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] string requestedByMemberId, CancellationToken cancellationToken);

    [Get("calendarevents/{calendarEventId}")]
    [AllowAnyStatusCode]
    Task<Response<CalendarEventSummary>> GetCalendarEventById(
        [Path] Guid calendarEventId, 
        [Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
        CancellationToken cancellationToken);
}
