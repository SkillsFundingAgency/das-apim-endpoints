using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.ApimDeveloper.Application.Users.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserQueryResult>
    {
        private readonly IApimDeveloperApiClient<ApimDeveloperApiConfiguration> _apiClient;

        public GetUserQueryHandler(IApimDeveloperApiClient<ApimDeveloperApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        
        public async Task<GetUserQueryResult> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var apiResponse =
                await _apiClient.GetWithResponseCode<GetUserResponse>(
                    new GetUserRequest(request.Email));

            if (!string.IsNullOrEmpty(apiResponse.ErrorContent))
            {
                if (apiResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    return new GetUserQueryResult();
                }
                throw new HttpRequestContentException($"Response status code does not indicate success: {(int)apiResponse.StatusCode} ({apiResponse.StatusCode})", apiResponse.StatusCode, apiResponse.ErrorContent);
            }
            
            return new GetUserQueryResult
            {
                User = apiResponse.Body
            };
        }
    }
}