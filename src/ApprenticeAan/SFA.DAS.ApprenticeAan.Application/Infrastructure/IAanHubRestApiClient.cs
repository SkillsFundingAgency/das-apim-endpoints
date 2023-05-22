using RestEase;
using SFA.DAS.ApprenticeAan.Application.Profiles.Queries.GetProfilesByUserType;
using SFA.DAS.ApprenticeAan.Application.Regions.Queries.GetRegions;

namespace SFA.DAS.ApprenticeAan.Application.Infrastructure;

public interface IAanHubRestApiClient
{
    public const string RequestedByMemberId = "X-RequestedByMemberId";

    [Get("/regions")]
    Task<GetRegionsQueryResult> GetRegions(CancellationToken cancellationToken);

    [Get("/profiles/{userType}")]
    Task<GetProfilesByUserTypeQueryResult> GetProfiles([Path] string userType, CancellationToken cancellationToken);



    /// [Get("calendarEvents")]
    /// [AllowAnyStatusCode]
    /// Task<Response<GetCalendarEventsQueryResult>> GetCalendarEvents(
    ///     [Header(ApiHeaders.RequestedByMemberId)] string requestedByMemberId);



}
