using MediatR;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.EmployerFinanceJobs.Queries.GetEmployerFundingProjectionByAccountId;

public class GetEmployerFundingProjectionByAccountIdQueryHandler(
    IFundingProjectionApiClient<FundingProjectionApiConfiguration> fundingProjectionApiClient) 
    : IRequestHandler<GetEmployerFundingProjectionByAccountIdQuery, GetEmployerFundingProjectionByAccountIdQueryResult>
{
    public async Task<GetEmployerFundingProjectionByAccountIdQueryResult> Handle(GetEmployerFundingProjectionByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var response = await fundingProjectionApiClient.Get<GetEmployerFundingProjectionByAccountIdResponse>(new GetEmployerFundingProjectionByAccountIdRequest(request.AccountId)); 
        return GetEmployerFundingProjectionByAccountIdQueryResult.ToResult(response);
    }
}