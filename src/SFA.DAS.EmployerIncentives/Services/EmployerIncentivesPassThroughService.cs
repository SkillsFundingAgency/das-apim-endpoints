using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Infrastructure.Api;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models.PassThrough;

namespace SFA.DAS.EmployerIncentives.Services
{
    public class EmployerIncentivesPassThroughService : IEmployerIncentivesPassThroughService
    {
        private readonly IPassThroughApiClient _client;

        public EmployerIncentivesPassThroughService(HttpClient httpClient, ILoggerFactory loggerFactory)
        {
            _client = new PassThroughApiClient(httpClient, loggerFactory.CreateLogger<PassThroughApiClient>());
        }
        public Task<InnerApiResponse> AddLegalEntity(long accountId, LegalEntityRequest legalEntityRequest, CancellationToken cancellationToken = default)
        {
            return _client.PostAsync($"/accounts/{accountId}/legalentities", legalEntityRequest, cancellationToken);
        }

        public Task<InnerApiResponse> RemoveLegalEntity(long accountId, long accountLegalEntityId, CancellationToken cancellationToken = default)
        {
            return _client.DeleteAsync($"/accounts/{accountId}/legalentities/{accountLegalEntityId}", cancellationToken);
        }
    }
}
