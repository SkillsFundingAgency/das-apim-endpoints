﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.Application.Pledges.Queries.GetPledges;

public class GetPledgesQueryHandler(ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> levyTransferMatchingApiClient)
    : IRequestHandler<GetPledgesQuery, GetPledgesQueryResult>
{
    public async Task<GetPledgesQueryResult> Handle(GetPledgesQuery request, CancellationToken cancellationToken)
    {
        var apiRequest = new GetPledgesRequest(request.AccountId);
        var response = await levyTransferMatchingApiClient.Get<GetPledgesResponse>(apiRequest);

        return new GetPledgesQueryResult
        {
            Pledges = response.Pledges.Select(p => new GetPledgesQueryResult.Pledge
            {
                Id = p.Id,
                AccountId = p.AccountId
            })
        };
    }
}