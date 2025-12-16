using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.GetMyApprenticeshipByUln
{
    public class GetMyApprenticeshipByUlnQueryHandler : IRequestHandler
        <GetMyApprenticeshipByUlnQuery, GetMyApprenticeshipByUlnQueryResult>
    {
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _accountsApiClient;

        public GetMyApprenticeshipByUlnQueryHandler(IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> accountsApiClient)
        {
            _accountsApiClient = accountsApiClient;
        }

        public async Task<GetMyApprenticeshipByUlnQueryResult> Handle
            (GetMyApprenticeshipByUlnQuery request, CancellationToken cancellationToken)
        {
            var myApprenticeship = await _accountsApiClient.Get<MyApprenticeship>(new GetMyApprenticeshipByUlnRequest(request.Uln));            

            return new GetMyApprenticeshipByUlnQueryResult { MyApprenticeship = myApprenticeship };
        }
    }
}
