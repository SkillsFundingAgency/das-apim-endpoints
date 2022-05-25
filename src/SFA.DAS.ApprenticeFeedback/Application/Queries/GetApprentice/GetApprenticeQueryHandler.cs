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
            var result = await _apiClient.Get<GetApprenticeResponse>(new GetApprenticeRequest(request.ApprenticeId));

            var apprenticePreferences =
                await _apiClient.Get<GetApprenticePreferencesResponse>(
                    new GetApprenticePreferencesRequest(request.ApprenticeId));

            if (result == null || apprenticePreferences.ApprenticePreferences.Count == 0)
                return null;

            return new GetApprenticeResult
            {
                ApprenticeId = result.ApprenticeId,
                FirstName = result.FirstName,
                LastName = result.LastName,
                DateOfBirth = result.DateOfBirth,
                Email = result.Email,
                IsPrivateBetaUser = result.IsPrivateBetaUser,
                TermsOfUseAccepted = result.TermsOfUseAccepted,
                ReacceptTermsOfUseRequired = result.ReacceptTermsOfUseRequired,
                ApprenticePreferences = apprenticePreferences.ApprenticePreferences
            };
        }
    }
}
