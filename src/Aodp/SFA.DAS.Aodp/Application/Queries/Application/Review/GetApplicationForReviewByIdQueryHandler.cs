using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.Aodp.Application.Constants;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using static SFA.DAS.Aodp.Application.Queries.Application.Review.GetApplicationForReviewByIdQueryResponse;

namespace SFA.DAS.Aodp.Application.Queries.Application.Review
{
    public class GetApplicationForReviewByIdQueryHandler : IRequestHandler<GetApplicationForReviewByIdQuery, BaseMediatrResponse<GetApplicationForReviewByIdQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
        private readonly IDfeUsersService _dfeUsersService;
        private readonly DfeSignInApiConfiguration _cfg;

        public GetApplicationForReviewByIdQueryHandler(
            IAodpApiClient<AodpApiConfiguration> apiClient, 
            IDfeUsersService dfeUsersService, 
            IOptions<DfeSignInApiConfiguration> cfg)
        {
            _apiClient = apiClient;
            _dfeUsersService = dfeUsersService;
            _cfg = cfg.Value;

        }

        public async Task<BaseMediatrResponse<GetApplicationForReviewByIdQueryResponse>> Handle(GetApplicationForReviewByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetApplicationForReviewByIdQueryResponse>();
            response.Success = false;
            try
            {
                var result = await _apiClient.GetWithResponseCode<GetApplicationForReviewByIdQueryResponse>(new GetApplicationReviewByIdApiRequest(request.ApplicationReviewId));
                result.EnsureSuccessStatusCode();

                var users = await _dfeUsersService.GetUsersByRoleAsync(_cfg.QfauUkprn, DfeRoles.Reviewer);

                response.Value = result.Body;
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

