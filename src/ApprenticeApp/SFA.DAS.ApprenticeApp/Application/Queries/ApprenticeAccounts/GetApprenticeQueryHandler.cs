using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeAccounts
{
    public class GetApprenticeQueryHandler : IRequestHandler<GetApprenticeQuery, GetApprenticeQueryResult>
    {
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _accountsApiClient;

        public GetApprenticeQueryHandler(IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> accountsApiClient)
            => _accountsApiClient = accountsApiClient;

        public async Task<GetApprenticeQueryResult> Handle(GetApprenticeQuery request, CancellationToken cancellationToken)
        {
            var result = await _accountsApiClient.Get<Apprentice>(new GetApprenticeRequest(request.ApprenticeId));

            return new GetApprenticeQueryResult { Apprentice = result };
        }
    }
}