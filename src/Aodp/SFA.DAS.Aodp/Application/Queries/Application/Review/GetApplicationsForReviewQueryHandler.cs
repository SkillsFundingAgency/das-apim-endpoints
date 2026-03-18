using MediatR;
using SFA.DAS.Aodp.Application.Constants;
using SFA.DAS.Aodp.Services;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using static SFA.DAS.Aodp.Application.Queries.Application.Review.GetApplicationForReviewByIdQueryResponse;

namespace SFA.DAS.Aodp.Application.Queries.Application.Review
{
    public class GetApplicationsForReviewQueryHandler : IRequestHandler<GetApplicationsForReviewQuery, BaseMediatrResponse<GetApplicationsForReviewQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
        private readonly IDfeUsersService _dfeUsersService;
        private readonly DfeSignInApiConfiguration _cfg;

        public GetApplicationsForReviewQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient,
            IDfeUsersService dfeUsersService,
            DfeSignInApiConfiguration cfg)
        {
            _apiClient = apiClient;
            _dfeUsersService = dfeUsersService;
            _cfg = cfg;
        }

        public async Task<BaseMediatrResponse<GetApplicationsForReviewQueryResponse>> Handle(GetApplicationsForReviewQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetApplicationsForReviewQueryResponse>();
            response.Success = false;
            try
            {
                var result = await _apiClient.PostWithResponseCode<GetApplicationsForReviewQueryResponse>(new GetApplicationsForReviewApiRequest()
                {
                    Data = request
                });
                result.EnsureSuccessStatusCode();

                response.Value = result.Body;

                var users = await _dfeUsersService.GetUsersByRoleAsync(_cfg.QfauUkprn, DfeRoles.Reviewer);
                response.Value.AvailableReviewers = users.Select(u => (Reviewer)u).ToList();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
}

