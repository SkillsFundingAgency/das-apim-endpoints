using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Infrastructure.Api;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Services
{
    public class EmployerIncentivesCommandPassThroughService : IEmployerIncentivesCommandPassThroughService
    {
        private readonly IPassThroughApiClient _client;

        public EmployerIncentivesCommandPassThroughService(HttpClient httpClient, ILoggerFactory loggerFactory)
        {
            _client = new PassThroughApiClient(httpClient, loggerFactory.CreateLogger<PassThroughApiClient>());
        }
        public Task<InnerApiResponse> PostAsync(string uri, CancellationToken cancellationToken = default)
        {
            return _client.PostAsync(uri, cancellationToken);
        }

        public Task<InnerApiResponse> PostAsync<TRequest>(string uri, TRequest request, CancellationToken cancellationToken = default) where TRequest : class
        {
            return _client.PostAsync(uri, request, cancellationToken);
        }

        public Task<InnerApiResponse> DeleteAsync(string uri, CancellationToken cancellationToken = default)
        {
            return _client.DeleteAsync(uri, cancellationToken);
        }
    }
}
