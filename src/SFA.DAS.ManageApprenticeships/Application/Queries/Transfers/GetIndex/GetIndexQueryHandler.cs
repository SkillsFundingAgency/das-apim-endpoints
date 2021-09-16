using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ManageApprenticeships.Application.Queries.Transfers.GetIndex
{
    public class GetIndexQueryHandler : IRequestHandler<GetIndexQuery, GetIndexQueryResult>
    {
        private readonly ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> _levyTransferMatchingApiClient;
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsApiClient;

        public GetIndexQueryHandler(ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> levyTransferMatchingApiClient, ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient)
        {
            _levyTransferMatchingApiClient = levyTransferMatchingApiClient;
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<GetIndexQueryResult> Handle(GetIndexQuery request, CancellationToken cancellationToken)
        {
            var pledgesTask = _levyTransferMatchingApiClient.Get<GetPledgesResponse>(new GetPledgesRequest(request.AccountId));
            var transferStatusTask = _commitmentsApiClient.Get<GetAccountTransferStatusResponse>(new GetAccountTransferStatusRequest(request.AccountId));

            await Task.WhenAll(pledgesTask, transferStatusTask);

            return new GetIndexQueryResult
            {
                PledgesCount = pledgesTask.Result.TotalPledges,
                IsTransferReceiver = transferStatusTask.Result.IsTransferReceiver,
                IsTransferSender = transferStatusTask.Result.IsTransferSender
            };
        }
    }
}
