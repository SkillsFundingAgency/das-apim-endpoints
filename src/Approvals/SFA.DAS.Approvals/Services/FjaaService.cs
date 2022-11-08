using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Services
{
    public interface IFjaaService
    {
        Task<bool> IsAccountLegalEntityOnFjaaRegister(long accountLegalEntityId);
    }

    public class FjaaService : IFjaaService
    {
        private readonly IFjaaApiClient<FjaaApiConfiguration> _fjaaClient;
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;
        private readonly ILogger<DeliveryModelService> _logger;

        public FjaaService(IFjaaApiClient<FjaaApiConfiguration> fjaaClient, ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient, ILogger<DeliveryModelService> logger)
        {
            _fjaaClient = fjaaClient;
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
            _logger = logger;
        }

        public async Task<bool> IsAccountLegalEntityOnFjaaRegister(long accountLegalEntityId)
        {
            _logger.LogInformation($"Requesting AccountLegalEntity {accountLegalEntityId} from Commitments v2 Api");
            var accountLegalEntity = await _commitmentsV2ApiClient.Get<GetAccountLegalEntityResponse>(new GetAccountLegalEntityRequest(accountLegalEntityId));

            _logger.LogInformation($"Requesting fjaa agency for LegalEntityId {accountLegalEntity.MaLegalEntityId}");
            var agencyRequest = await _fjaaClient.GetWithResponseCode<GetAgencyResponse>(new GetAgencyRequest(accountLegalEntity.MaLegalEntityId));

            if (agencyRequest.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }

            agencyRequest.EnsureSuccessStatusCode();
            return true;
        }

    }
}
