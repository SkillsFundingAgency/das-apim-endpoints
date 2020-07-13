using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Infrastructure.Api;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Services
{
    public class EmployerIncentivesQueryPassThroughService : IEmployerIncentivesQueryPassThroughService
    {
        private readonly IPassThroughApiClient _client;

        public EmployerIncentivesQueryPassThroughService(HttpClient httpClient, ILoggerFactory loggerFactory)
        {
            _client = new PassThroughApiClient(httpClient, loggerFactory.CreateLogger<PassThroughApiClient>());
        }

        public Task<InnerApiResponse> GetAsync(Uri uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            return _client.GetAsync(uri, queryData, cancellationToken);
        }

        public Task<InnerApiResponse> GetAsync(string uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            return _client.GetAsync(uri, queryData, cancellationToken);
        }
    }
}
