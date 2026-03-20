using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.Application.Review
{
    public class SaveQanCommandHandler : IRequestHandler<SaveQanCommand, BaseMediatrResponse<SaveQanCommandResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


        public SaveQanCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<SaveQanCommandResponse>> Handle(SaveQanCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<SaveQanCommandResponse>()
            {
                Success = false,
                Value = new SaveQanCommandResponse()
            };

            try
            {
                var apiRequest = new SaveQanApiRequest(request.ApplicationReviewId)
                {
                    Data = request
                };
                var result = await _apiClient.PutWithResponseCode<SaveQanCommandResponse>(apiRequest);

                response.Value.IsQanValid = result.Body.IsQanValid;
                response.Value.QanValidationMessage = result.Body.QanValidationMessage;

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