using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Accounts.Queries.GetAccountQuery
{
    public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, GetAccountResult>
    {
        private readonly IAccountsApiClient<AccountsConfiguration> _apiClient;

        public GetAccountQueryHandler(IAccountsApiClient<AccountsConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetAccountResult> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetAccountResponse>(new GetAccountRequest(request.HashedAccountId));

            if (result == null)
                return null;

            return new GetAccountResult
            {
                AccountId = result.AccountId,
                HashedAccountId = result.HashedAccountId,
                PublicHashedAccountId = result.PublicHashedAccountId,
                DasAccountName = result.DasAccountName,
                DateRegistered = result.DateRegistered,
                OwnerEmail = result.OwnerEmail,
                Balance = result.Balance,
                RemainingTransferAllowance = result.RemainingTransferAllowance,
                StartingTransferAllowance = result.StartingTransferAllowance,
                AccountAgreementType = result.AccountAgreementType,
                ApprenticeshipEmployerType = result.ApprenticeshipEmployerType,
                IsAllowedPaymentOnService = result.IsAllowedPaymentOnService
            };
        }
    }
}