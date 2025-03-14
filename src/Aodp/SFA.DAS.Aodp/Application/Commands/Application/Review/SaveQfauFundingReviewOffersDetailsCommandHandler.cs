using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.Application.Review
{
    public class SaveQfauFundingReviewOffersDetailsCommandHandler : IRequestHandler<SaveQfauFundingReviewOffersDetailsCommand, BaseMediatrResponse<EmptyResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


        public SaveQfauFundingReviewOffersDetailsCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<EmptyResponse>> Handle(SaveQfauFundingReviewOffersDetailsCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<EmptyResponse>()
            {
                Success = false
            };

            try
            {
                var apiRequest = new SaveQfauFundingReviewOffersDetailsApiRequest()
                {
                    ApplicationReviewId = request.ApplicationReviewId,
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