using MediatR;
using SFA.DAS.AODP.Application.Commands.Application.Review;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.Application.Review
{
    public class SaveReviewerCommandHandler : IRequestHandler<SaveReviewerCommand, BaseMediatrResponse<SaveReviewerCommandResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


        public SaveReviewerCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
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