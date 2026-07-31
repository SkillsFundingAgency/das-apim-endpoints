using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.Aodp.Services;
namespace SFA.DAS.Aodp.Application.Commands.Rollover
{
    public class ValidateRolloverExtensionCommandHandler
        : IRequestHandler<ValidateRolloverExtensionCommand, BaseMediatrResponse<ValidateRolloverExtensionCommandResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public ValidateRolloverExtensionCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;

        }

        public async Task<BaseMediatrResponse<ValidateRolloverExtensionCommandResponse>> Handle(ValidateRolloverExtensionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<ValidateRolloverExtensionCommandResponse>();

            try
            {
                var result = await _apiClient.PostWithResponseCode<ValidateRolloverExtensionCommandResponse>(new ValidateRolloverExtensionApiRequest()
                {
                    Data = request
                });

                response.Value = result.Body;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
}
