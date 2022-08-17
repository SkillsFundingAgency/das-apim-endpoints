using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetCounts
{
    public class GetCountsQueryHandler : IRequestHandler<GetCountsQuery, GetCountsQueryResult>
    {
        private readonly ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> _levyTransferMatchingApiClient;

        public GetCountsQueryHandler(ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> levyTransferMatchingApiClient)
        {
            _levyTransferMatchingApiClient = levyTransferMatchingApiClient;
        }

        public async Task<GetCountsQueryResult> Handle(GetCountsQuery request, CancellationToken cancellationToken)
        {
            var pledgesTask = _levyTransferMatchingApiClient.Get<GetPledgesResponse>(new GetPledgesRequest(request.AccountId));

            var applicationsTask = _levyTransferMatchingApiClient.Get<GetApplicationsResponse>(new GetApplicationsRequest
            {
                AccountId = request.AccountId
            });

            await Task.WhenAll(pledgesTask, applicationsTask);

            return new GetCountsQueryResult
            {
                PledgesCount = pledgesTask.Result.TotalPledges,
                ApplicationsCount = applicationsTask.Result.Applications.Count()
            };
        }
    }
}
