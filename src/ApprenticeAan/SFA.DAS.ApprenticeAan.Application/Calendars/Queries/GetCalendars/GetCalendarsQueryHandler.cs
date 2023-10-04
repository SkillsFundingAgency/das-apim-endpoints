using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.Model;

namespace SFA.DAS.ApprenticeAan.Application.Calendars.Queries.GetCalendars;

public class GetCalendarsQueryHandler : IRequestHandler<GetCalendarsQuery, IEnumerable<Calendar>>
{
    private readonly IAanHubRestApiClient _apiClient;

    public GetCalendarsQueryHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IEnumerable<Calendar>> Handle(GetCalendarsQuery request, CancellationToken cancellationToken)
    {
        return await _apiClient.GetCalendars(cancellationToken);

    }
}