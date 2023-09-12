using RestEase;
using SFA.DAS.EmployerAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.EmployerAan.Application.Employer.Commands.CreateEmployerMember;
using SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMember;
using SFA.DAS.EmployerAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
using SFA.DAS.EmployerAan.Application.Members.Queries.GetMember;
using SFA.DAS.EmployerAan.Application.Members.Queries.GetMembers;
using SFA.DAS.EmployerAan.Application.Profiles.Queries.GetProfilesByUserType;
using SFA.DAS.EmployerAan.Application.Regions.Queries.GetRegions;
using SFA.DAS.EmployerAan.InnerApi.Attendances;
using SFA.DAS.EmployerAan.InnerApi.Notifications;
using SFA.DAS.EmployerAan.InnerApi.MemberProfiles;
using SFA.DAS.EmployerAan.Models;

namespace SFA.DAS.EmployerAan.Infrastructure;

public interface IAanHubRestApiClient
{
    public const string RequestedByMemberId = Constants.ApiHeaders.RequestedByMemberIdHeader;

    [Get("/profiles/{userType}")]
    Task<GetProfilesByUserTypeQueryResult> GetProfiles([Path] string userType, CancellationToken cancellationToken);

    [Get("/regions")]
    Task<GetRegionsQueryResult> GetRegions(CancellationToken cancellationToken);

    [Get("/calendars")]
    Task<List<Calendar>> GetCalendars(CancellationToken cancellationToken);

    [Get("/calendarEvents")]
    Task<GetCalendarEventsQueryResult> GetCalendarEvents([Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, [QueryMap] IDictionary<string, string[]> parameters, CancellationToken cancellationToken);

    [Get("/employers/{userRef}")]
    [AllowAnyStatusCode]
    Task<Response<GetEmployerMemberQueryResult?>> GetEmployer([Path] Guid userRef, CancellationToken cancellationToken);

    [Post("/employers")]
    Task<CreateEmployerMemberCommandResult> PostEmployerMember([Body] CreateEmployerMemberCommand command, CancellationToken cancellationToken);

    [Get("attendances")]
    Task<GetAttendancesResponse> GetAttendances(
        [Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
        string FromDate,
        string ToDate,
        CancellationToken cancellationToken);

    [Get("members")]
    Task<GetMembersQueryResult> GetMembers([QueryMap] IDictionary<string, string[]> parameters, CancellationToken cancellationToken);

    [Get("members/{memberId}")]
    Task<GetMemberQueryResult> GetMember([Path] Guid memberId, CancellationToken cancellationToken);

    [Get("/notifications/{id}")]
    Task<Response<GetNotificationResponse?>> GetNotification([Path] Guid id, [Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
    CancellationToken cancellationToken);

    [Get("calendarEvents/{calendarEventId}")]
    [AllowAnyStatusCode]
    Task<Response<CalendarEvent>> GetCalendarEventById(
    [Path] Guid calendarEventId,
    [Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
    CancellationToken cancellationToken);

    [Put("/CalendarEvents/{calendarEventId}/attendance")]
    Task PutAttendance(
    [Path] Guid calendarEventId,
    [Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
    [Body] AttendanceStatus putAttendanceRequest,
    CancellationToken cancellationToken);

    
    [Get("/members/{memberId}/profile")]
    Task<GetMemberProfileWithPreferencesQueryResult> GetMemberProfileWithPreferences([Path] Guid memberId, [Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, bool @public, CancellationToken cancellationToken);

    
    [Put("members/{memberId}/profile")]
    Task PutMemberProfile(
       [Path] Guid memberId,
       [Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
       [Body] UpdateMemberProfileModel updateMemberProfileRequest,
       CancellationToken cancellationToken);
}
