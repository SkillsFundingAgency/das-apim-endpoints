using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.AparRegister.InnerApi.Requests;
using SFA.DAS.AparRegister.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AparRegister.Application.Queries.ProviderStatusEvents;

public class GetProviderStatusEventsQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> _apiClient) : IRequestHandler<GetProviderStatusEventsQuery, IEnumerable<ProviderStatusEvent>>
{
    public async Task<IEnumerable<ProviderStatusEvent>> Handle(GetProviderStatusEventsQuery request, CancellationToken cancellationToken)
    {
        var response = await _apiClient.GetWithResponseCode<IEnumerable<ProviderStatusEvent>>(new GetProviderStatusEventsRequest(request.SinceEventId, request.PageSize, request.PageNumber));

        response.EnsureSuccessStatusCode();

        return response.Body;
    }
}
