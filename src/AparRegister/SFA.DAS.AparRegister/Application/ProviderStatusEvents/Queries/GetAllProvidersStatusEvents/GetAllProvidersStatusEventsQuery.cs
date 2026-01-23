using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.AparRegister.InnerApi.Requests;
using SFA.DAS.AparRegister.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AparRegister.Application.ProviderStatusEvents.Queries.GetAllProvidersStatusEvents;

public record GetAllProvidersStatusEventsQuery(int SinceEventId, int PageSize, int PageNumber) : IRequest<GetAllProvidersStatusEventsQueryResult>;

public record ProviderStatusEvent(long Id, int ProviderId, string Event, DateTime CreatedOn);

public record GetAllProvidersStatusEventsQueryResult(List<ProviderStatusEvent> Events);

public class GetAllProvidersStatusEventsQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> _apiClient) : IRequestHandler<GetAllProvidersStatusEventsQuery, GetAllProvidersStatusEventsQueryResult>
{
    public async Task<GetAllProvidersStatusEventsQueryResult> Handle(GetAllProvidersStatusEventsQuery request, CancellationToken cancellationToken)
    {
        var response = await _apiClient.GetWithResponseCode<GetOrganisationStatusEventsResponse>(new GetOrganisationStatusEventsRequest(request.SinceEventId, request.PageSize, request.PageNumber));

        throw new NotImplementedException();
    }
}

