using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeAan.Api.Configuration;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Profiles.Requests;
using SFA.DAS.ApprenticeAan.Application.Services;

namespace SFA.DAS.ApprenticeAan.Application.Profiles.Queries.GetProfilesByUserType
{
    public class GetProfilesByUserTypeQueryHandler : IRequestHandler<GetProfilesByUserTypeQuery, GetProfilesByUserTypeQueryResult?>
    {
        private readonly IAanHubApiClient<AanHubApiConfiguration> _apiClient;
        private readonly ILogger<GetProfilesByUserTypeQueryHandler> _logger;
        public GetProfilesByUserTypeQueryHandler(IAanHubApiClient<AanHubApiConfiguration> apiClient, ILogger<GetProfilesByUserTypeQueryHandler> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }
        public async Task<GetProfilesByUserTypeQueryResult?> Handle(GetProfilesByUserTypeQuery request, CancellationToken cancellationToken)
        {
            var profilesResult = await _apiClient.GetWithResponseCode<GetProfilesByUserTypeQueryResult>(new GetProfilesByUserTypeQueryRequest(request.UserType));

            if (profilesResult.StatusCode == HttpStatusCode.OK && profilesResult.Body != null)
                return new GetProfilesByUserTypeQueryResult
                {
                    ProfileModels = profilesResult.Body.ProfileModels
                };
            _logger.LogError("ApprenticeAan Outer API: Unable to query AanHub API /Profiles endpoint");
            return null;
        }
    }
}