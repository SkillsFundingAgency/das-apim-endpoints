using System.Net;
using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

using SFA.DAS.Apim.Shared.Models;

namespace SFA.DAS.SharedOuterApi.Types.Services
{
	public class CollectionCalendarApiClient : ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>
	{
		private readonly IInternalApiClient<CollectionCalendarApiConfiguration> _apiClient;

		public CollectionCalendarApiClient(IInternalApiClient<CollectionCalendarApiConfiguration> apiClient)
		{
			_apiClient = apiClient;
		}

		public Task<TResponse> Get<TResponse>(IGetApiRequest request)
		{
			return _apiClient.Get<TResponse>(request);
		}

		public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
		{
			return _apiClient.GetResponseCode(request);
		}

		public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
		{
			throw new NotImplementedException();
		}
	}
}
