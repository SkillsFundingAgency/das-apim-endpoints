using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprentice
{
    public class GetApprenticeQueryHandler : IRequestHandler<GetApprenticeQuery, GetApprenticeResult>
    {
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _apiClient;

        public GetApprenticeQueryHandler(IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetApprenticeResult> Handle(GetApprenticeQuery request, CancellationToken cancellationToken)
        {
            var result =  _apiClient.Get<GetApprenticeResponse>(new GetApprenticeRequest(request.ApprenticeId));

            var apprenticePreferences = _apiClient.Get<GetApprenticePreferencesResponse>(
                    new GetApprenticePreferencesRequest(request.ApprenticeId));

            await Task.WhenAll(result, apprenticePreferences);

            if (result.Result == null)
                return null;

            return new GetApprenticeResult
            {
                ApprenticeId = result.Result.ApprenticeId,
                FirstName = result.Result.FirstName,
                LastName = result.Result.LastName,
                DateOfBirth = result.Result.DateOfBirth,
                Email = result.Result.Email,
                TermsOfUseAccepted = result.Result.TermsOfUseAccepted,
                ReacceptTermsOfUseRequired = result.Result.ReacceptTermsOfUseRequired,
                ApprenticePreferences = apprenticePreferences.Result.ApprenticePreferences
            };
        }
    }
}
