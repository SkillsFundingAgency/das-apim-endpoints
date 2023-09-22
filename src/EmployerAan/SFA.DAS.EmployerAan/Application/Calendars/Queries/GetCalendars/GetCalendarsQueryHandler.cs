using MediatR;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.EmployerAan.Models;

namespace SFA.DAS.EmployerAan.Application.Calendars.Queries.GetCalendars;
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
