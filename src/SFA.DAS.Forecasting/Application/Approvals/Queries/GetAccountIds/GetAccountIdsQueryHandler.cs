using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Forecasting.InnerApi.Requests;
using SFA.DAS.Forecasting.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.Application.Approvals.Queries.GetAccountIds
{
    public class GetAccountIdsQueryHandler : IRequestHandler<GetAccountIdsQuery, GetAccountIdsQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2Api;

        public GetAccountIdsQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2Api)
        {
            _commitmentsV2Api = commitmentsV2Api;
        }

        public async Task<GetAccountIdsQueryResult> Handle(GetAccountIdsQuery request, CancellationToken cancellationToken)
        {
            var result = await _commitmentsV2Api.Get<GetAccountsWithCohortsResponse>(new GetAccountsWithCohortsRequest());

            return new GetAccountIdsQueryResult
            {
                AccountIds = result.AccountIds
            };
        }
    }
}