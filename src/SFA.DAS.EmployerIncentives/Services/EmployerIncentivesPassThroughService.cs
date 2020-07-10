using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Infrastructure.Api;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models.PassThrough;

namespace SFA.DAS.EmployerIncentives.Services
{
    public class EmployerIncentivesPassThroughService : IEmployerIncentivesPassThroughService
    {
        private readonly IPassThroughApiClient _client;

        public EmployerIncentivesPassThroughService(HttpClient httpClient)
        {
            _client = new PassThroughApiClient(httpClient);
        }

        public Task<InnerApiResponse> AddLegalEntity(long accountId, LegalEntityRequest legalEntityRequest)
        {
            return _client.PostAsync($"/accounts/{accountId}/legalentities", legalEntityRequest, true, CancellationToken.None);
        }

        public Task<InnerApiResponse> RemoveLegalEntity(long accountId, long accountLegalEntityId)
        {
            return _client.DeleteAsync($"/accounts/{accountId}/legalentities/{accountLegalEntityId}", false, CancellationToken.None);
        }
    }
}
