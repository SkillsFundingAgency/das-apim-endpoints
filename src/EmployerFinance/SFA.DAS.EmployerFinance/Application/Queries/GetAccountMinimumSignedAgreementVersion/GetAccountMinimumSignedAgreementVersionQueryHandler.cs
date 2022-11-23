using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetAccountMinimumSignedAgreementVersion
{
    public class GetAccountMinimumSignedAgreementVersionQueryHandler : IRequestHandler<GetAccountMinimumSignedAgreementVersionQuery, GetAccountMinimumSignedAgreementVersionQueryResult>
    {
        private readonly IAccountsApiClient<AccountsConfiguration> _accountsApiClient;

        public GetAccountMinimumSignedAgreementVersionQueryHandler(IAccountsApiClient<AccountsConfiguration> accountsApiClient)
        {
            _accountsApiClient = accountsApiClient;
        }

        public async Task<GetAccountMinimumSignedAgreementVersionQueryResult> Handle(GetAccountMinimumSignedAgreementVersionQuery request, CancellationToken cancellationToken)
        {
            var minimumSignedAgreementVersion = await _accountsApiClient.Get<int>(new GetAccountMinimumSignedAgreementVersionRequest(request.AccountId));
            return new GetAccountMinimumSignedAgreementVersionQueryResult { MinimumSignedAgreementVersion = minimumSignedAgreementVersion };
        }
    }
}
