using System.Net;
using System.Threading.Tasks;
using SFA.DAS.Campaign.Configuration;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Campaign.ExternalApi
{
    public class ContentfulPreviewApiClient : IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration>
    {
        private readonly IContentfulApiClient<ContentfulPreviewApiConfiguration> _client;

        public ContentfulPreviewApiClient (IContentfulApiClient<ContentfulPreviewApiConfiguration> client)
        {
            _client = client;
        }
        
        public Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            return _client.Get<TResponse>(request);
        }

        public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
        {
            return _client.GetResponseCode(request);
        }

        public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
        {
            return _client.GetWithResponseCode<TResponse>(request);
        }
    }
}