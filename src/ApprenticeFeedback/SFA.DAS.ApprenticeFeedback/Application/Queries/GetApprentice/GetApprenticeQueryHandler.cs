using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

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
            var apprentice = _apiClient.Get<GetApprenticeResponse>(new GetApprenticeRequest(request.ApprenticeId));

            var apprenticePreferences = _apiClient.Get<GetApprenticePreferencesResponse>(
                new GetApprenticePreferencesRequest(request.ApprenticeId));

            await Task.WhenAll(apprentice, apprenticePreferences);

            if (apprentice.Result == null)
                return null;

            return new GetApprenticeResult
            {
                ApprenticeId = apprentice.Result.ApprenticeId,
                FirstName = apprentice.Result.FirstName,
                LastName = apprentice.Result.LastName,
                DateOfBirth = apprentice.Result.DateOfBirth,
                Email = apprentice.Result.Email,
                TermsOfUseAccepted = apprentice.Result.TermsOfUseAccepted,
                ReacceptTermsOfUseRequired = apprentice.Result.ReacceptTermsOfUseRequired,
                ApprenticePreferences = apprenticePreferences.Result.ApprenticePreferences
            };
        }
    }
}