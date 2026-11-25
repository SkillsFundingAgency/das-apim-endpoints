using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsStatusByAccountId;

public class GetLevyProjectionsStatusByAccountIdQueryHandler(
    IFundingProjectionApiClient<FundingProjectionApiConfiguration> projectionApiClient)
    : IRequestHandler<GetLevyProjectionsStatusByAccountIdQuery, GetLevyProjectionsStatusByAccountIdQueryResult>
{
    public async Task<GetLevyProjectionsStatusByAccountIdQueryResult> Handle(GetLevyProjectionsStatusByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var response = await projectionApiClient.GetWithResponseCode<GetImportJobStateResponse>(new GetImportJobStateRequest(request.AccountId));

        if (response.StatusCode == HttpStatusCode.NotFound)
            return new GetLevyProjectionsStatusByAccountIdQueryResult(DateTime.MinValue);

        response.EnsureSuccessStatusCode();

        return new GetLevyProjectionsStatusByAccountIdQueryResult(response.Body.LastSuccessfulImportDate);
    }
}
