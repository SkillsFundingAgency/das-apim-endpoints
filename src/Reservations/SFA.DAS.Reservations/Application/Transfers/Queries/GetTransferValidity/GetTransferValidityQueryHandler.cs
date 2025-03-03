﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Reservations.InnerApi.Requests;
using SFA.DAS.Reservations.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.Application.Transfers.Queries.GetTransferValidity
{
    public class GetTransferValidityQueryHandler : IRequestHandler<GetTransferValidityQuery, GetTransferValidityQueryResult>
    {
        private readonly IFinanceApiClient<FinanceApiConfiguration> _financeApiClient;
        private readonly ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> _levyTransferMatchingApiClient;

        private const string PledgeApplicationAcceptedStatus = "Accepted";

        public GetTransferValidityQueryHandler(IFinanceApiClient<FinanceApiConfiguration> financeApiClient, ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> levyTransferMatchingApiClient)
        {
            _financeApiClient = financeApiClient;
            _levyTransferMatchingApiClient = levyTransferMatchingApiClient;
        }
        public async Task<GetTransferValidityQueryResult> Handle(GetTransferValidityQuery request, CancellationToken cancellationToken)
        {
            if (request.PledgeApplicationId.HasValue)
            {
                var pledgeApplicationRequest = new GetPledgeApplicationRequest(request.PledgeApplicationId.Value);
                var pledgeApplication = await _levyTransferMatchingApiClient.Get<GetPledgeApplicationResponse>(pledgeApplicationRequest);

                return new GetTransferValidityQueryResult
                {
                    IsValid = pledgeApplication.Status == PledgeApplicationAcceptedStatus
                };
            }

            var apiRequest = new GetTransferConnectionsRequest(request.ReceiverId);
            var connections = await _financeApiClient.GetAll<TransferConnection>(apiRequest);
            return new GetTransferValidityQueryResult
            {
                IsValid = connections.Any(x => x.FundingEmployerAccountId == request.SenderId)
            };
        }
    }
}