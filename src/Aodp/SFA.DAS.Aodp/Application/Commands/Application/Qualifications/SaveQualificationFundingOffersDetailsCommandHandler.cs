using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;

namespace SFA.DAS.Aodp.Application.Commands.Application.Qualifications
{
    public class SaveQualificationFundingOffersDetailsCommandHandler : IRequestHandler<SaveQualificationFundingOffersDetailsCommand, BaseMediatrResponse<EmptyResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


        public SaveQualificationFundingOffersDetailsCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<EmptyResponse>> Handle(SaveQualificationFundingOffersDetailsCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<EmptyResponse>()
            {
                Success = false
            };

            try
            {
                var apiRequest = new SaveQualificationFundingOffersDetailsApiRequest()
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