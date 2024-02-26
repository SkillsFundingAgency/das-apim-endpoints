using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Services
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
			throw new NotImplementedException();
		}

		public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
		{
			throw new NotImplementedException();
		}
	}
}
