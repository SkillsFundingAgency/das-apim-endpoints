using MediatR;
using SFA.DAS.Aodp.Application.Commands.Application.Application;
using SFA.DAS.Aodp.Application.Commands.Application.Review;
using SFA.DAS.Aodp.InnerApi.Application.Review;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Application.Commands.Application.Review
{
    public class BulkSaveReviewerCommandHandler
        : IRequestHandler<BulkSaveReviewerCommand, BaseMediatrResponse<BulkSaveReviewerCommandResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public BulkSaveReviewerCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
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
                var result = await _apiClient.PutWithResponseCode<BulkSaveReviewerCommandResponse>(
                    new BulkSaveReviewerApiRequest()
                    {
                        Data = request
                    });

                response.Value = result.Body;
                response.Success = true;
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
