using MediatR;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticePortal.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticePortal.Application.ApprenticeAccounts.Queries
{
    public class GetCurrentApprenticeshipQueryHandler : IRequestHandler<GetCurrentApprenticeshipQuery, GetCurrentApprenticeshipQueryResult>
    {
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _accountsApiClient;

        public GetCurrentApprenticeshipQueryHandler(IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> accountsApiClient)
            => _accountsApiClient = accountsApiClient;

        public async Task<GetCurrentApprenticeshipQueryResult> Handle(GetCurrentApprenticeshipQuery request, CancellationToken cancellationToken)
        {
            var currentApprenticeship = _accountsApiClient.Get<CurrentApprenticeship>(new GetCurrentApprenticeshipRequest(request.ApprenticeId));
            await Task.WhenAll(currentApprenticeship);

            return new GetCurrentApprenticeshipQueryResult { Apprenticeship = await currentApprenticeship};
        }
    }
}