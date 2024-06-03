using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.CreateAccount.SignIntoYourOldAccount;

public class GetSignIntoYourOldAccountQueryHandler(
    IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration> legacyApiClient)
    : IRequestHandler<GetSignIntoYourOldAccountQuery, GetSignIntoYourOldAccountQueryResult>
{
    public async Task<GetSignIntoYourOldAccountQueryResult> Handle(GetSignIntoYourOldAccountQuery request, CancellationToken cancellationToken)
    {
        var result =
            await legacyApiClient.Get<GetLegacyValidateCredentialsApiResponse>(
                new GetLegacyValidateCredentialsApiRequest(request.Email, request.Password));

        return new GetSignIntoYourOldAccountQueryResult
        {
            IsValid = result.IsValid
        };
    }
}