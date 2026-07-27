using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.Application.UkrlpData.Queries.GetUkrlpProviders;

public class GetUkrlpProvidersQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> _roatpServiceApiClient)
    : IRequestHandler<GetUkrlpProvidersQuery, GetUkrlpProvidersQueryResult>
{
    private const int MaximumRecords = 100;

    public async Task<GetUkrlpProvidersQueryResult> Handle(GetUkrlpProvidersQuery query, CancellationToken cancellationToken)
    {
        var ukprns = query.Ukprns;

        var response = new List<ProviderDetails>();

        IEnumerable<int[]> batches = ukprns.Chunk(MaximumRecords);

        var tasks = batches.Select(batch => ProcessUkrlpRequest(query.UpdatedSinceDate, batch));
        var results = await Task.WhenAll(tasks);

        response.AddRange(results.SelectMany(r => r));

        return new GetUkrlpProvidersQueryResult(response);
    }

    private async Task<IEnumerable<ProviderDetails>> ProcessUkrlpRequest(DateTime? updatedSinceDate, IEnumerable<int> ukprns)
    {
        GetUkrlpProvidersRequest request = new() { UpdatedSinceDate = updatedSinceDate, Ukprns = ukprns };

        var response = await _roatpServiceApiClient.GetWithResponseCode<UkrlpProvidersResponse>(request);

        response.EnsureSuccessStatusCode();

        return response.Body.Providers.Select(p => (ProviderDetails)p);
    }
}
