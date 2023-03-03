using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeAan.Api.Configuration;
using SFA.DAS.ApprenticeAan.Application.Entities;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Profiles.Requests;
using SFA.DAS.ApprenticeAan.Application.Services;

namespace SFA.DAS.ApprenticeAan.Application.Profiles.Queries.GetProfiles
{
    //public class GetProfilesQueryHandler1 : IRequestHandler<GetProfilesByUserType, List<ProfileModel>>
    //{
    //    private readonly IAanHubApiClient<AanHubApiConfiguration> _apiClient;
    //    private readonly ILogger<GetProfilesQueryHandler1> _logger;
    //    public GetProfilesQueryHandler1(IAanHubApiClient<AanHubApiConfiguration> apiClient, ILogger<GetProfilesQueryHandler1> logger)
    //    {
    //        _apiClient = apiClient;
    //        _logger = logger;
    //    }
    //    public async Task<List<ProfileModel>> Handle(GetProfilesByUserType request, CancellationToken cancellationToken)
    //    {
    //        var profilesResult = await _apiClient.GetWithResponseCode<List<ProfileModel>>(new GetProfilesQueryRequest(request.UserType));
    //        List<ProfileModel> profilesByUserType;

    //        if (profilesResult.StatusCode == HttpStatusCode.OK && profilesResult.Body != null)
    //        {
    //            profilesByUserType = profilesResult.Body;
    //            return profilesByUserType;
    //        }
    //        _logger.LogError("ApprenticeAan Outer API: Unable to query AanHub API /Regions endpoint");
    //        return null;
    //    }
    //}
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
            var profilesResult = await _apiClient.GetWithResponseCode<GetProfilesByUserTypeQueryResult>(new GetProfilesQueryRequest(request.UserType));
            List<ProfileModel> profilesByUserType;

            if (profilesResult.StatusCode == HttpStatusCode.OK && profilesResult.Body != null)
            {
                profilesByUserType = profilesResult.Body.Profiles;
                return profilesByUserType;
            }
            _logger.LogError("ApprenticeAan Outer API: Unable to query AanHub API /Profiles endpoint");
            return null;
        }
    }
}
