using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Files;
using SFA.DAS.Aodp.Services;
namespace SFA.DAS.Aodp.Application.Queries.Files
{

    public class GetFileMetadataQueryHandler : IRequestHandler<GetFileMetadataQuery, BaseMediatrResponse<GetFileMetadataQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public GetFileMetadataQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;

        }


        public async Task<BaseMediatrResponse<GetFileMetadataQueryResponse>> Handle(GetFileMetadataQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetFileMetadataQueryResponse>() { Success = false };

            try
            {

                var metadata = await _apiClient.PostWithResponseCode<GetFileMetadataQueryResponse>(new GetFileMetadataApiRequest() { Data = request });

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
