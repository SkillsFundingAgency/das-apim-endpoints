using RestEase;
using SFA.DAS.ApprenticeAan.Application.Regions.Queries.GetRegions;
using static SFA.DAS.ApprenticeAan.Application.Common.Constants;

namespace SFA.DAS.ApprenticeAan.Application.Infrastructure;

public interface IAanHubRestApiClient
{
    [Get(AanHubApiUrls.GetRegionsUrl)]
    Task<GetRegionsQueryResult> GetRegions();


    /// [Get("calendarEvents")]
    /// [AllowAnyStatusCode]
    /// Task<Response<GetCalendarEventsQueryResult>> GetCalendarEvents(
    ///     [Header(ApiHeaders.RequestedByMemberId)] string requestedByMemberId);



}
