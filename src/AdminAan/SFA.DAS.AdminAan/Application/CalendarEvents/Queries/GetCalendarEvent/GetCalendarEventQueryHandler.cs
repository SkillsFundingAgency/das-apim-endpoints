using MediatR;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvent;
public class GetCalendarEventQueryHandler : IRequestHandler<GetCalendarEventQuery, GetCalendarEventQueryResult?>
{
    private readonly IAanHubRestApiClient _apiClient;
    private readonly IReferenceDataApiClient _apiReferenceDataApiClient;
    public GetCalendarEventQueryHandler(IAanHubRestApiClient apiClient, IReferenceDataApiClient apiReferenceDataApiClient)
    {
        _apiClient = apiClient;
        _apiReferenceDataApiClient = apiReferenceDataApiClient;
    }

    public async Task<GetCalendarEventQueryResult?> Handle(GetCalendarEventQuery request, CancellationToken cancellationToken)
    {
        var result = await _apiClient.GetCalendarEvent(request.RequestedByMemberId, request.CalendarEventId, cancellationToken);

        if (result?.RegionId != null)
        {
            var regionsResult = await _apiClient.GetRegions(cancellationToken);
            result.RegionName = regionsResult.Regions.FirstOrDefault(x => x.Id == result.RegionId)?.Area;
        }

        if (string.IsNullOrEmpty(result?.Urn.ToString())) return result;

        try
        {
            var organisationTypeEducational = 4;

            using var response = await _apiReferenceDataApiClient.GetSchoolFromUrn(result.Urn.ToString()!,
                organisationTypeEducational, cancellationToken);

            if (response.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result.SchoolName = response.GetContent()?.Name;
            }
        }
        catch
        {
            return result;
        }

        return result;
    }
}

