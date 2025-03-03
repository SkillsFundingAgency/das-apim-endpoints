using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Interfaces;
using ApprenticeshipEmployerType = SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts.ApprenticeshipEmployerType;

namespace SFA.DAS.Approvals.Application.AgreementNotSigned.Queries;

public class
    GetAgreementNotSignedQueryHandler : IRequestHandler<GetAgreementNotSignedQuery, GetAgreementNotSignedResult>
{
    private readonly IAccountsApiClient<AccountsConfiguration> _apiClient;

    public GetAgreementNotSignedQueryHandler(IAccountsApiClient<AccountsConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetAgreementNotSignedResult> Handle(GetAgreementNotSignedQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _apiClient.Get<GetAccountByIdResponse>(new GetAccountByIdRequest(request.AccountId));

        if (result == null)
            return null;

        return new GetAgreementNotSignedResult
        {
            AccountId = result.AccountId,
            IsLevyAccount = result.ApprenticeshipEmployerType == ApprenticeshipEmployerType.Levy
        };
    }
}
