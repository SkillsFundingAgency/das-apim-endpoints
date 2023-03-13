using MediatR;
using SFA.DAS.ApprenticeAan.Api.Configuration;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Profiles.Requests;
using SFA.DAS.ApprenticeAan.Application.Services;

namespace SFA.DAS.ApprenticeAan.Application.Profiles.Queries.GetProfilesByUserType
{
    public class GetProfilesByUserTypeQueryHandler : IRequestHandler<GetProfilesByUserTypeQuery, GetProfilesByUserTypeQueryResult?>
    {
        private readonly IAanHubApiClient<AanHubApiConfiguration> _apiClient;
        public GetProfilesByUserTypeQueryHandler(IAanHubApiClient<AanHubApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetProfilesByUserTypeQueryResult?> Handle(GetProfilesByUserTypeQuery request, CancellationToken cancellationToken)
        {
            return await _apiClient.Get<GetProfilesByUserTypeQueryResult>(new GetProfilesByUserTypeQueryRequest(request.UserType));
        }
    }
}