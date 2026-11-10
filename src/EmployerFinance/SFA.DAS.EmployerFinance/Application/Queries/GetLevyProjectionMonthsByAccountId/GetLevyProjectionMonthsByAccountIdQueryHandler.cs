using MediatR;
using SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionMonthsByAccountId;

public class GetLevyProjectionMonthsByAccountIdQueryHandler(
    IFundingProjectionApiClient<FundingProjectionApiConfiguration> projectionApiClient)
    : IRequestHandler<GetLevyProjectionMonthsByAccountIdQuery, GetLevyProjectionMonthsByAccountIdQueryResult>
{
    public async Task<GetLevyProjectionMonthsByAccountIdQueryResult> Handle(GetLevyProjectionMonthsByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var response = await projectionApiClient.Get<GetProjectionMonthsResponse>(new GetProjectionMonthsRequest(request.AccountId))
            ?? throw new InvalidOperationException($"Unable to get the projection months for account {request.AccountId}");

        return new GetLevyProjectionMonthsByAccountIdQueryResult(response.Months);
    }
}
