using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;

namespace SFA.DAS.Aodp.Application.Commands.Application.Review
{
    public class SaveReviewOwnerUpdateCommandHandler : IRequestHandler<SaveReviewOwnerUpdateCommand, BaseMediatrResponse<EmptyResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


        public SaveReviewOwnerUpdateCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<EmptyResponse>> Handle(SaveReviewOwnerUpdateCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<EmptyResponse>()
            {
                Success = false
            };

            try
            {
                var apiRequest = new SaveReviewOwnerUpdateApiRequest(request.ApplicationReviewId)
                {
                    Data = request
                };
                await _apiClient.Put(apiRequest);
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