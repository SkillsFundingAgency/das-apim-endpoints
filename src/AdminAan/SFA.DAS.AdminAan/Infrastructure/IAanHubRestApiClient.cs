using MediatR;
using RestEase;
using SFA.DAS.AdminAan.Application.Admins.Commands.Create;
using SFA.DAS.AdminAan.Application.Admins.Queries.Lookup;
using SFA.DAS.AdminAan.Application.CalendarEvents.Commands.Create;
using SFA.DAS.AdminAan.Application.CalendarEvents.Commands.Update;
using SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvent;
using SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.AdminAan.Application.Entities;
using SFA.DAS.AdminAan.Application.Regions.Queries.GetRegions;
using SFA.DAS.AdminAan.Domain;

namespace SFA.DAS.AdminAan.Infrastructure;

public interface IAanHubRestApiClient
{
    [Get("/regions")]
    Task<GetRegionsQueryResult> GetRegions(CancellationToken cancellationToken);


    [Get("/calendars")]
    Task<List<Calendar>> GetCalendars(CancellationToken cancellationToken);


    [Get("calendarEvents")]
    Task<GetCalendarEventsQueryResult> GetCalendarEvents([Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, [QueryMap] IDictionary<string, string[]> parameters, CancellationToken cancellationToken);

    [Post("calendarEvents")]
    Task<PostEventCommandResult> PostCalendarEvents([Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, [Body] PostEventCommand command, CancellationToken cancellationToken);

    [Put("calendarEvents/{calendarEventId}")]
    Task PutCalendarEvent([Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, [Path] Guid calendarEventId, [Body] PutEventCommand command, CancellationToken cancellationToken);


    [Get("calendarEvents/{calendarEventId}")]
    [AllowAnyStatusCode]
    Task<GetCalendarEventQueryResult> GetCalendarEvent([Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, [Path] Guid calendarEventId, CancellationToken cancellationToken);

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
    Task<Response<LookupAdminMemberResult>> GetMemberByEmail([Path] string email, CancellationToken cancellationToken);

    [Post("/admins")]
    [AllowAnyStatusCode]
    Task<CreateAdminMemberCommandResult> CreateAdminMember([Body] CreateAdminMemberCommand command, CancellationToken cancellationToken);
}

