using MediatR;
using SFA.DAS.Aodp.Application.Commands.Rollover;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.Aodp.Services;

namespace SFA.DAS.AODP.Application.Commands.Rollover
{
    public class ApplyFundingExtensionsCommandHandler
        : IRequestHandler<ApplyFundingExtensionsCommand, BaseMediatrResponse<ApplyFundingExtensionsCommandResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
        public ApplyFundingExtensionsCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<ApplyFundingExtensionsCommandResponse>> Handle(
            ApplyFundingExtensionsCommand request, 
            CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<ApplyFundingExtensionsCommandResponse>();

            try
            {
                var result = await _apiClient.PostWithResponseCode<ApplyFundingExtensionsCommandResponse>(new ApplyFundingExtensionsApiRequest()
                {
                    Data = request
                });

                response.Value =  result.Body;
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
