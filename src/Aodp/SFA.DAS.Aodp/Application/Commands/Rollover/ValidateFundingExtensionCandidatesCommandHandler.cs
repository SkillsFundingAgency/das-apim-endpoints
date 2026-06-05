using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.Aodp.Services;
namespace SFA.DAS.Aodp.Application.Commands.Rollover
{
    public class ValidateFundingExtensionCandidatesCommandHandler
        : IRequestHandler<ValidateFundingExtensionCandidatesCommand, BaseMediatrResponse<ValidateFundingExtensionCandidatesCommandResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public ValidateFundingExtensionCandidatesCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;

        }

        public async Task<BaseMediatrResponse<ValidateFundingExtensionCandidatesCommandResponse>> Handle(ValidateFundingExtensionCandidatesCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<ValidateFundingExtensionCandidatesCommandResponse>();

            try
            {
                var result = await _apiClient.PostWithResponseCode<ValidateFundingExtensionCandidatesCommandResponse>(new ValidateFundingExtensionCandidatesApiRequest()
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
