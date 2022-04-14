using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ManageApprenticeships.Application.Queries.Transfers.GetIndex
{
    public class GetIndexQueryHandler : IRequestHandler<GetIndexQuery, GetIndexQueryResult>
    {
        private readonly ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> _levyTransferMatchingApiClient;

        public GetIndexQueryHandler(ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> levyTransferMatchingApiClient)
        {
            _levyTransferMatchingApiClient = levyTransferMatchingApiClient;
        }

        public async Task<GetIndexQueryResult> Handle(GetIndexQuery request, CancellationToken cancellationToken)
        {
            var pledgesTask = _levyTransferMatchingApiClient.Get<GetPledgesResponse>(new GetPledgesRequest(request.AccountId));

            var applicationsTask = _levyTransferMatchingApiClient.Get<GetApplicationsResponse>(new GetApplicationsRequest
            {
                AccountId = request.AccountId
            });

            await Task.WhenAll(pledgesTask, applicationsTask);

            return new GetIndexQueryResult
            {
                PledgesCount = pledgesTask.Result.TotalPledges,
                ApplicationsCount = applicationsTask.Result.Applications.Count()
            };
        }
    }
}
