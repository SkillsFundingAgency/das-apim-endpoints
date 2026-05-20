using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.Application.Review;
using SFA.DAS.Aodp.Services;

namespace SFA.DAS.AODP.Application.Commands.Application.Review
{
    public class BulkSaveReviewerCommandHandler
        : IRequestHandler<BulkSaveReviewerCommand, BaseMediatrResponse<BulkSaveReviewerCommandResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
        private readonly ILogger<BulkSaveReviewerCommandHandler> _logger;

        public BulkSaveReviewerCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient,
            ILogger<BulkSaveReviewerCommandHandler> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<BaseMediatrResponse<BulkSaveReviewerCommandResponse>> Handle(
     BulkSaveReviewerCommand request,
     CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<BulkSaveReviewerCommandResponse>()
            {
                Success = false
            };

            try
            {
                _logger.LogInformation(
                    "OUTER API HANDLER: Sending BulkSaveReviewer to inner API. Count={Count}, Reviewer1Set={Reviewer1Set}, Reviewer1={Reviewer1}, Reviewer2Set={Reviewer2Set}, Reviewer2={Reviewer2}, UserType={UserType}",
                    request?.ApplicationReviewIds?.Count,
                    request?.Reviewer1Set,
                    request?.Reviewer1,
                    request?.Reviewer2Set,
                    request?.Reviewer2,
                    request?.UserType);

                var apiRequest = new BulkSaveReviewerApiRequest()
                {
                    Data = request
                };

                _logger.LogInformation(
                    "OUTER API HANDLER: Inner API PUT Url={PutUrl}",
                    apiRequest.PutUrl);

                var result = await _apiClient.PutWithResponseCode<BulkSaveReviewerCommandResponse>(apiRequest);

                _logger.LogInformation(
                    "OUTER API HANDLER: Inner API call completed. StatusCode={StatusCode}, HasBody={HasBody}",
                    result.StatusCode,
                    result.Body != null);

                if (result.Body != null)
                {
                    _logger.LogInformation(
                        "OUTER API HANDLER: Inner API response body. RequestedCount={RequestedCount}, UpdatedCount={UpdatedCount}, ErrorCount={ErrorCount}",
                        result.Body.RequestedCount,
                        result.Body.UpdatedCount,
                        result.Body.ErrorCount);
                }

                response.Value = result.Body;
                response.Success = (int)result.StatusCode >= 200 && (int)result.StatusCode < 300;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "OUTER API HANDLER: Exception calling inner API for BulkSaveReviewer.");

                response.ErrorMessage = ex.Message;
                response.Success = false;
            }

            return response;
        }
    }
}
