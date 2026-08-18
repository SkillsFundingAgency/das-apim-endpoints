using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Files;
using SFA.DAS.Aodp.Services;
namespace SFA.DAS.Aodp.Application.Commands.Files
{

    public class CreateFileMetadataCommandHandler : IRequestHandler<CreateFileMetadataCommand, BaseMediatrResponse<EmptyResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public CreateFileMetadataCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;

        }

        public async Task<BaseMediatrResponse<EmptyResponse>> Handle(CreateFileMetadataCommand command, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<EmptyResponse>() { Success = false };

            try
            {
                var metadata = await _apiClient.PostWithResponseCode<EmptyResponse>(new CreateFileMetadataApiRequest() { Data = command });

                response.Value = metadata.Body;
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
