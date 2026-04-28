using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Aodp.Application.Constants;
using SFA.DAS.Aodp.Services;
using SFA.DAS.Aodp.Configuration;

using static SFA.DAS.Aodp.Application.Queries.Application.Review.GetApplicationForReviewByIdQueryResponse;
using SFA.DAS.Apim.Shared.Extensions;

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
                _logger.LogInformation("Calling AODP inner API for application reviews");
                var result = await _apiClient.PostWithResponseCode<GetApplicationsForReviewQueryResponse>(
                    new GetApplicationsForReviewApiRequest { Data = request });

                result.EnsureSuccessStatusCode();
                _logger.LogInformation("AODP inner API call succeeded");

                response.Value = result.Body;

                _logger.LogInformation("Calling DfE users service for reviewers. Ukprn: {Ukprn}", _cfg.QfauUkprn);
                var users = await _dfeUsersService.GetUsersByRoleAsync(_cfg.QfauUkprn, DfeRoles.Reviewer);
                _logger.LogInformation("DfE users service call succeeded. Users returned: {Count}", users.Count);

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

