using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.Application.Review
{
    public class SaveQualificationFundingOffersOutcomeCommandHandler : IRequestHandler<SaveQualificationFundingOffersOutcomeCommand, BaseMediatrResponse<EmptyResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


        public SaveQualificationFundingOffersOutcomeCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<EmptyResponse>> Handle(SaveQualificationFundingOffersOutcomeCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<EmptyResponse>()
            {
                Success = false
            };

            try
            {
                var apiRequest = new SaveQualificationFundingOffersOutcomeApiRequest()
                {
                    QualificationVersionId = request.QualificationVersionId,
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