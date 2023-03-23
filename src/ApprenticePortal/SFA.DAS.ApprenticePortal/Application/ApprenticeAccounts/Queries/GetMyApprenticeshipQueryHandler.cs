﻿using MediatR;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticePortal.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticePortal.Application.ApprenticeAccounts.Queries
{
    public class GetMyApprenticeshipQueryHandler : IRequestHandler<GetMyApprenticeshipQuery, GetMyApprenticeshipQueryResult>
    {
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _accountsApiClient;

        public GetMyApprenticeshipQueryHandler(IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> accountsApiClient)
            => _accountsApiClient = accountsApiClient;

        public async Task<GetMyApprenticeshipQueryResult> Handle(GetMyApprenticeshipQuery request, CancellationToken cancellationToken)
        {
            var myApprenticeship = _accountsApiClient.Get<MyApprenticeshipData>(new GetMyApprenticeshipRequest(request.ApprenticeId));
            await Task.WhenAll(myApprenticeship);

            return new GetMyApprenticeshipQueryResult { ApprenticeshipData = await myApprenticeship};
        }
    }
}