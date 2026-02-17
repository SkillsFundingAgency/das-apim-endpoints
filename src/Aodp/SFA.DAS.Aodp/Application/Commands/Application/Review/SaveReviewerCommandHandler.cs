using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.Application.Review
{
    public class SaveReviewerCommandHandler : IRequestHandler<SaveReviewerCommand, BaseMediatrResponse<SaveReviewerCommandResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
        private readonly ILogger<SaveReviewerCommandHandler> _logger;

        public SaveReviewerCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient, ILogger<SaveReviewerCommandHandler> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<BaseMediatrResponse<SaveReviewerCommandResponse>> Handle(SaveReviewerCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<SaveReviewerCommandResponse>()
            {
                Success = false
            };

            try
            {
                var apiRequest = new SaveReviewerApiRequest(request.ApplicationId)
                {
                    Data = request
                };

                var result = await _apiClient.PutWithResponseCode<SaveReviewerCommandResponse>(apiRequest);
                response.Success = true;
                response.Value = result.Body;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.Success = false;
            }

            return response;
        }
    }
}