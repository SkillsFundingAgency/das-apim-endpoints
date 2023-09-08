using RestEase;
using SFA.DAS.AdminAan.Application.Admins.Commands.Create;
using SFA.DAS.AdminAan.Application.Admins.Queries.Lookup;
using SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.AdminAan.Application.Entities;
using SFA.DAS.AdminAan.Application.Regions.Queries.GetRegions;
using SFA.DAS.AdminAan.Infrastructure.Configuration;

namespace SFA.DAS.AdminAan.Infrastructure;

public interface IAanHubRestApiClient
{
    [Get("/regions")]
    Task<GetRegionsQueryResult> GetRegions(CancellationToken cancellationToken);


    [Get("/calendars")]
    Task<List<Calendar>> GetCalendars(CancellationToken cancellationToken);


    [Get("calendarEvents")]
    Task<GetCalendarEventsQueryResult> GetCalendarEvents([Header(Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, [QueryMap] IDictionary<string, string[]> parameters, CancellationToken cancellationToken);

    [Get("/members/{email}")]
    [AllowAnyStatusCode]
    Task<Response<LookupAdminMemberResult>> GetMemberByEmail([Path] string email, CancellationToken cancellationToken);

    [Post("/admins")]
    [AllowAnyStatusCode]
    Task<CreateAdminMemberCommandResult> CreateAdminMember([Body] CreateAdminMemberCommand command, CancellationToken cancellationToken);
}

