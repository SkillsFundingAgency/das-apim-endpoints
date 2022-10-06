using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.ApprenticeFeedback.Models;
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
            var apprenticeResult = _apiClient.GetWithResponseCode<GetApprenticeResponse>(new GetApprenticeRequest(request.ApprenticeId));
            
            var apprenticePreferencesResult = _apiClient.GetWithResponseCode<GetApprenticePreferencesResponse>(new GetApprenticePreferencesRequest(request.ApprenticeId));

            await Task.WhenAll(apprenticeResult, apprenticePreferencesResult);

            if (apprenticeResult.Result.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new ApprenticeNotFoundException($"Apprentice with id:{request.ApprenticeId} not found");
            }
            else if(apprenticeResult.Result.StatusCode != System.Net.HttpStatusCode.OK || apprenticeResult.Result.Body == null)
            {
                return null;
            }

            var apprentice = apprenticeResult.Result.Body;
            var apprenticePreferences = apprenticePreferencesResult.Result.Body;
            
            return new GetApprenticeResult
            {
                ApprenticeId = apprentice.ApprenticeId,
                FirstName = apprentice.FirstName,
                LastName = apprentice.LastName,
                DateOfBirth = apprentice.DateOfBirth,
                Email = apprentice.Email,
                TermsOfUseAccepted = apprentice.TermsOfUseAccepted,
                ReacceptTermsOfUseRequired = apprentice.ReacceptTermsOfUseRequired,
                ApprenticePreferences = apprenticePreferences?.ApprenticePreferences ?? new System.Collections.Generic.List<ApprenticePreferenceDto>()
            };
        }
    }
}