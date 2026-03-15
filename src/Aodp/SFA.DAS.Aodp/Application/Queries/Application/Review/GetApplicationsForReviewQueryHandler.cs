using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Aodp.Application.Constants;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using static SFA.DAS.Aodp.Application.Queries.Application.Review.GetApplicationForReviewByIdQueryResponse;

namespace SFA.DAS.Aodp.Application.Queries.Application.Review
{
    public class GetApplicationsForReviewQueryHandler : IRequestHandler<GetApplicationsForReviewQuery, BaseMediatrResponse<GetApplicationsForReviewQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
        private readonly IDfeUsersService _dfeUsersService;
        private readonly DfeSignInApiConfiguration _cfg;
        private readonly ILogger<GetApplicationsForReviewQueryHandler> _logger;

        public GetApplicationsForReviewQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient,
            IDfeUsersService dfeUsersService,
            DfeSignInApiConfiguration cfg,
            ILogger<GetApplicationsForReviewQueryHandler> logger)
        {
            _apiClient = apiClient;
            _dfeUsersService = dfeUsersService;
            _cfg = cfg;
            _logger = logger;
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
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed calling a downstream service");
                response.ErrorMessage = "A dependent service is unavailable.";
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Failed retrieving reviewer users");
                response.ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error retrieving application reviews");
                response.ErrorMessage = "An unexpected error occurred.";
            }

            return response;
        }
    }
}

